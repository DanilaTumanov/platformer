using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    
    private PlayerController Hero;
    private Rect statsRect;
    private Rect healthPos;
    private Rect minesPos;

    // Use this for initialization
    void Start () {
        statsRect = new Rect(15, 15, 80, 38);
        healthPos = new Rect(20, 15, 100, 30);
        minesPos = new Rect(20, 30, 100, 30);
        Hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void OnGUI () {
        
        GUI.Box(statsRect, "");
        GUI.Label(healthPos, "Health: " + Hero.HP);
        GUI.Label(minesPos, "Mines: " + Hero.trapNumber);

    }
}
