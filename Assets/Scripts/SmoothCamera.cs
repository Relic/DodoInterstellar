using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class SmoothCamera : MonoBehaviour
{
    #region Constants
    const float CAMERA_LERP = 0.1f;
    const float CAMERA_Z = -10.0f;
    const float LAYER_SCALE_FACTOR = 0.3f;
    const float BG_MOVE_OFFSET = 0.0275f;
    #endregion

    public GameObject cameraPrefab;
    public GameObject defaultBackgroundPrefab;

    public Vector3 CameraPosition { get { return _camera.transform.position; } }

    private LevelManager _lm;
    private Camera _camera;
    private float _cameraHeight;
    private Vector2 _cameraSize;

    private List<GameObject> _backgrounds = new List<GameObject>();
    private List<Vector3> _backgroundStartPositions = new List<Vector3>();
    private int _nextBackgroundId = 1;

    void Awake()
    {
        _lm = GetComponent<LevelManager>();
    }

    void Update()
    {
        // Lerp the camera position towards the player position, locking the z position.
        Vector3 newPos = Vector3.Lerp(_camera.transform.position, _lm.PlayerPosition, CAMERA_LERP);
        newPos.z = CAMERA_Z;
        _camera.transform.position = newPos;

        for (int i = 0; i < _backgrounds.Count; ++i)
        {
            Vector3 newBgPos = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0.0f);
            Vector3 offset = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0.0f) * BG_MOVE_OFFSET * i;
            _backgrounds[i].transform.position = _backgroundStartPositions[i] + newBgPos - offset;
        }
    }

    public Camera CreateCamera()
    {
        if (_camera == null)
        {
            _camera = Instantiate(cameraPrefab).GetComponent<Camera>();
            _camera.transform.position = new Vector3(_lm.PlayerPosition.x, _lm.PlayerPosition.y, _lm.PlayerPosition.z);
            _cameraHeight = _camera.orthographicSize * 2.0f;
            _cameraSize = new Vector2(_cameraHeight * _camera.aspect, _cameraHeight);
            SetupInitialBackground();
        }
        return _camera.GetComponent<Camera>();
    }

    public void ClearBackgrounds()
    {
        if (_backgrounds.Count > 1)
        {
            for (int i = 1; i < _backgrounds.Count; ++i)
            {
                Destroy(_backgrounds[i]);
                _backgroundStartPositions[i] = Vector3.zero;
            }
        }
    }
    public void AddBackground(string spriteName)
    {
        GameObject background = Instantiate(defaultBackgroundPrefab);
        SpriteRenderer sr = background.GetComponent<SpriteRenderer>();

        Vector3 screenPos = new Vector3(Random.Range(Screen.width * 0.2f, Screen.width * 0.8f), Random.Range(Screen.height * 0.2f, Screen.height * 0.8f), _camera.transform.position.z);
        sr.transform.position = _camera.ScreenToWorldPoint(screenPos);
        sr.transform.rotation = Quaternion.Euler(Random.Range(0, 15), Random.Range(0, 15), Random.Range(0, 360));

        List<Sprite> sprites = Resources.LoadAll<Sprite>("Background").ToList<Sprite>();
        sprites = sprites.Where(sprite => sprite.name.StartsWith(spriteName)).ToList<Sprite>();

        sr.sprite = sprites[Random.Range(0, sprites.Count - 1)];
        sr.transform.localScale *= LAYER_SCALE_FACTOR * _nextBackgroundId;
        sr.sortingOrder = _nextBackgroundId++;

        if (spriteName.StartsWith("bg_galaxy"))
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Random.Range(0.1f, 0.25f));
        }

        _backgrounds.Add(background);
        _backgroundStartPositions.Add(sr.transform.position);
    }

    private void SetupInitialBackground()
    {
        _backgrounds.Add(Instantiate(defaultBackgroundPrefab));
        _backgroundStartPositions.Add(Vector3.zero);

        // Camera scaling of background image
        // Credit to: https://kylewbanks.com/blog/create-fullscreen-background-image-in-unity2d-with-spriterenderer
        SpriteRenderer sr = _backgrounds[0].GetComponent<SpriteRenderer>();

        Vector2 spriteSize = sr.sprite.bounds.size;

        Vector2 scale = _backgrounds[0].transform.localScale;
        if (_cameraSize.x > _cameraSize.y)
        {
            scale *= _cameraSize.x / spriteSize.x;
        }
        else
        {
            scale *= _cameraSize.y / spriteSize.y;
        }
        _backgrounds[0].transform.localScale = scale;
    }

    public void SnapCamera(Vector3 position)
    {
        _camera.transform.position = position;
    }
}
