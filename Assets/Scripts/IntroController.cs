using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour {
    public TMPro.TextMeshProUGUI text;

    void Start()
    {
        int _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

        LevelImporter importer = new LevelImporter();
        LevelsCollection _levels = importer.loadMetadata(Resources.Load<TextAsset>("levels").text);

        if (_levels.levels[_currentLevel].introText != null)
        {
            text.text = _levels.levels[_currentLevel].introText;
        } else
        {
            SceneManager.LoadScene("Game");
        }
    }
    void Update () {
		if (Input.anyKey)
        {
            SceneManager.LoadScene("Game");
        }
	}
}
