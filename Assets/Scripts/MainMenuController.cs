using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Transform planet;
    private Vector3 start;
    private Vector3 des;

    public void OnClickStartGame()
    {
        SceneManager.LoadScene("Intro");
    }

    public void OnClickCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnClickExit()
    {
        PlayerPrefs.DeleteKey("CurrentLevel");
        Application.Quit();
    }

    void Start()
    {
        start = new Vector3(planet.position.x, planet.position.y, planet.position.z);
        des = new Vector3(-600, planet.position.y, planet.position.z);

    }

    void Update()
    {
            // planet.position = Vector3.Lerp(start, des, Time.deltaTime * 0.1f);
    }
}
