using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {


    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
