using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {


    private Canvas LoadScreen;



    private void Start()
    {
        LoadScreen = GameObject.Find("LoadScreen").GetComponent<Canvas>();
    }



    public void StartGame()
    {
        LoadScreen.enabled = true;
        SceneManager.LoadSceneAsync("Level1");
    }

    public void Exit()
    {
        Application.Quit();
    }




}
