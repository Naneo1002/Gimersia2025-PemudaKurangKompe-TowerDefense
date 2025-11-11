using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartNewgame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void NewGameMenu()
    {
        SceneManager.LoadScene("NewGame");
    }

    public void EndlessMode() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("EndlessMode");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
