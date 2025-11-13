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

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay1");
    }

    public void Credit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Credit");
    }

    public void NextPage()
    {
        SceneManager.LoadScene("HowToPlay2");
    }
    
    public void PreviousPage()
    {
        SceneManager.LoadScene("HowToPlay1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
