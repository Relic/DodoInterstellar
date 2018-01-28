using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Tooltip("The prefab for the player controller.")]
    public GameObject PlayerControllerPrefab;
    public TextMeshProUGUI levelName;
    public GameObject rewindButton;

    public Vector3 PlayerPosition { get { return _playerController.transform.position; } }

    private float _fixedTime = 0.0f;

    private InputFrame _prevInputFrame = InputFrame.Empty;
    private InputFrame _inputFrame = InputFrame.Empty;

    private Timeline _currentTimeline;
    private List<Timeline> _timelines = new List<Timeline>();

    private PlayerController _playerController;
    private Camera _camera;
    private LevelsCollection _levels;
    private int _currentLevel = 0;
    private Vector3 _currentPlayerStartPosition = Vector3.zero;

    private bool _canMove = false;
    private bool _rewindNextFixedUpdate = false;

    #region Test Input
    private bool _z = false;
    private bool _x = false;
    private bool _c = false;
    private bool _f = false;
    #endregion

    void Start()
    {
        _playerController = Instantiate(PlayerControllerPrefab).GetComponent<PlayerController>();
        _playerController.IsMainPlayer = true;
        if (_currentPlayerStartPosition != Vector3.zero)
        {
            _playerController.transform.position = _currentPlayerStartPosition;
            _playerController.StartPosition = _currentPlayerStartPosition;
        }
        _camera = GetComponent<SmoothCamera>().CreateCamera();
        CreateNewTimelineNow();

        LoadLevel();

        SetFrozen(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene("MainMenu");
        if (!_canMove) return;

        bool mouseDown = Input.GetMouseButton(0);

        Vector3 mousePosition = mouseDown ? Input.mousePosition : Vector3.zero;
        if (mouseDown)
        {
            mousePosition = _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _camera.transform.position.z));
        }

        _prevInputFrame = _inputFrame;
        _inputFrame = new InputFrame(_fixedTime, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), mouseDown, mousePosition);
    }

    void FixedUpdate()
    {
        _fixedTime += Time.fixedDeltaTime;

        if (_rewindNextFixedUpdate) ForceRewind();
        if (_canMove)
        {
            if (_currentTimeline != null && (_prevInputFrame != _inputFrame || _inputFrame != InputFrame.Empty)) _currentTimeline.RecordFrame(_inputFrame);

            _playerController.Move(_inputFrame);
        }
        PhantomUpdate();
    }

    public IEnumerator Freeze(float time)
    {
        SetFrozen(true);
        yield return new WaitForSeconds(time);
        SetFrozen(false);
    }

    public void SetFrozen(bool frozen)
    {
        _canMove = !frozen;
        if (frozen)
        {
            _playerController.SetVelocity(Vector2.zero);
            _inputFrame = InputFrame.Empty;
        }
    }

    private void ForceRewind()
    {
        ReplayAllTimelines(0.0f);
        CreateNewTimelineStart();
        _rewindNextFixedUpdate = false;
    }

    public void RewindNextFixedUpdate()
    {
        _rewindNextFixedUpdate = true;
    }

    /// <summary>
    /// Check if this is the main player object (the one being controlled by the user)
    /// </summary>
    /// <param name="pc"></param>
    /// <returns></returns>
    public bool IsMainPlayerObject(GameObject obj)
    {
        return obj.GetComponent<PlayerController>().IsMainPlayer;
    }

    public void LevelDone()
    {
        if (_currentLevel + 1 == _levels.levels.Length)
        {
            PlayerPrefs.DeleteKey("CurrentLevel");
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", _currentLevel + 1);
            SceneManager.LoadScene("Intro");
        }
    }

    private void LoadLevel()
    {
        _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

        LevelImporter importer = new LevelImporter();
        _levels = importer.loadMetadata(Resources.Load<TextAsset>("levels").text);

        if (levelName != null) levelName.text = _levels.levels[_currentLevel].title;
        rewindButton.SetActive(_levels.levels[_currentLevel].rewindCount != 0);

        if (_camera != null)
        {
            SmoothCamera sc = GetComponent<SmoothCamera>();
            sc.ClearBackgrounds();
            for (int i = 0; i < _levels.levels[_currentLevel].background.Length; ++i)
            {
                sc.AddBackground(_levels.levels[_currentLevel].background[i]);
            }
        }
        LoadLevelObjects(importer);
    }

    private void LoadLevelObjects(LevelImporter importer)
    {
        List<LevelObject> objects = importer.loadLevel(Resources.Load<TextAsset>(PlayerPrefs.GetInt("CurrentLevel", 0).ToString()).text);
        LevelObjectManager levelObjectManager = GetComponent<LevelObjectManager>();
        LevelsCollection levels = importer.loadMetadata(Resources.Load<TextAsset>("levels").text);
        LevelsCollection.Level level = levels.levels[PlayerPrefs.GetInt("CurrentLevel", 0)];
        levelObjectManager.GridSize = new Vector2Int(level.gridSizeX, level.gridSizeY);
        for (int i = 0; i < objects.Count; i++)
        {
            levelObjectManager.InstantiateLevelObject(objects[i]);
        }
    }

    /// <summary>
    /// Update movement of any timelines being replayed
    /// </summary>
    private void PhantomUpdate()
    {
        for (int i = 0; i < _timelines.Count; ++i)
        {
            if (_timelines[i].Playing) _timelines[i].TryPlayNextFrame(_fixedTime);
        }
    }

    /// <summary>
    /// Start a new timeline and begin recording
    /// </summary>
    private void CreateNewTimelineNow()
    {
        _currentTimeline = new Timeline(_fixedTime, _playerController.transform.position);
        _timelines.Add(_currentTimeline);
    }

    /// <summary>
    /// Start a new timeline from the start of the level and begin recording
    /// </summary>
    private void CreateNewTimelineStart()
    {
        _playerController.transform.position = _playerController.StartPosition;
        _fixedTime = 0.0f;
        _currentTimeline = new Timeline(_fixedTime, _playerController.transform.position);
        _timelines.Add(_currentTimeline);
    }

    /// <summary>
    /// Replay a timeline starting at a certain time
    /// </summary>
    /// <param name="timelineId">The timeline to replay</param>
    /// <param name="startTime">The time the replay starts from</param>
    private void ReplayTimeline(int timelineId, float startTime)
    {
        if (timelineId >= _timelines.Count) return;

        GameObject phantom = Instantiate(PlayerControllerPrefab);
        phantom.GetComponentInChildren<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        _timelines[timelineId].StartReplay(startTime, phantom.GetComponent<PlayerController>());
    }

    /// <summary>
    /// Replay all timelines simultaneously starting at a certain time
    /// </summary>
    /// <param name="startTime">The time the replay starts from</param>
    private void ReplayAllTimelines(float startTime)
    {
        for (int i = 0; i < _timelines.Count; ++i)
        {
            ReplayTimeline(i, startTime);
        }
    }

    /// <summary>
    /// Sets the current players start position
    /// </summary>
    /// <param name="position"></param>
    public void SetPlayersStartPosition(Vector3 position)
    {
        Vector3 newStartPosition = new Vector3(position.x, position.y, transform.position.z);
        _currentPlayerStartPosition = newStartPosition;
        if (_playerController != null)
        {
            _playerController.transform.position = newStartPosition;
            _playerController.StartPosition = newStartPosition;
        }
        if (_currentTimeline != null)
        {
            _currentTimeline.StartPosition = newStartPosition;
        }
    }
}
