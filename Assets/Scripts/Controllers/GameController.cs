using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    Canvas GameUI;
    Canvas PauseScreen;
    Canvas GameOverScreen;
    Text GameOverText;

    Door door;
    PlayerController hero;

    bool paused = false;



    public bool Paused
    {
        get
        {
            return paused;
        }
    }



    private void Start()
    {
        GameUI = GameObject.Find("GameUI").GetComponent<Canvas>();
        PauseScreen = GameObject.Find("PauseScreen").GetComponent<Canvas>();
        GameOverScreen = GameObject.Find("GameOverScreen").GetComponent<Canvas>();
        GameOverText = GameObject.Find("GameOverText").GetComponent<Text>();

        door = GameObject.FindGameObjectWithTag("Door").GetComponent<Door>();
        hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<PlayerController>();

        door.OnOpen += OnDoorOpen;
        hero.OnDeath += OnHeroDeath;
    }

    private void Update()
    {
        if (!door.Opened && Input.GetButtonDown("Cancel"))
        {
            paused = !paused;
            PauseScreen.enabled = paused;
            Time.timeScale = paused ? 0 : 1;
        }
    }


    private void OnDoorOpen()
    {
        SetGameOverScreen("You WIN!");
    }


    private void OnHeroDeath()
    {
        SetGameOverScreen("GAME OVER");
    }


    private void SetGameOverScreen(string text)
    {
        GameOverText.text = text;
        GameOverScreen.enabled = true;
        paused = true;
        Time.timeScale = 0;
    }

}
