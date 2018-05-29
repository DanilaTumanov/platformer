using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {


    PlayerController hero;
    Weapon primaryWeapon;

    Image primaryWeaponIcon;
    Image primaryWeaponFade;

    Text trapCount;

	// Use this for initialization
	void Start () {
        hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<PlayerController>();
        primaryWeapon = hero.GetComponentInChildren<Weapon>();
        
        primaryWeaponIcon = GameObject.Find("PrimaryWeaponIcon").GetComponent<Image>();
        primaryWeaponFade = GameObject.Find("PrimaryWeaponFade").GetComponent<Image>();

        trapCount = GameObject.Find("TrapCount").GetComponent<Text>();

        primaryWeaponIcon.sprite = primaryWeapon.UIicon;
    }
	
	// Update is called once per frame
	void Update () {
        
        ProcessPrimaryWeaponStats();

        ProcessTrapsStats();

    }


    private void ProcessPrimaryWeaponStats()
    {
        if (primaryWeapon.IsReloading())
        {
            //primaryWeaponFade.fillAmount = (Time.time - primaryWeapon.LastShotTime) / primaryWeapon.cooldown;
            primaryWeaponFade.fillAmount += Time.deltaTime / primaryWeapon.cooldown;
        }
        else
        {
            primaryWeaponFade.fillAmount = 0;
        }
    }

    private void ProcessTrapsStats()
    {
        trapCount.text = "X" + hero.trapNumber;
    }
}
