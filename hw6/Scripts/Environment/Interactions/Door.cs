using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {


    private PlayerController hero = null;
    private bool opened = false;
    private bool needAKey = false;
    private Rect msgRect;


    private void Start()
    {
        msgRect = new Rect(Screen.width / 2 - 60, Screen.height / 2 - 15, 120, 30);
    }


    private void Update()
    {
        if(hero != null)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (hero.HasKey())
                {
                    DoorOpen();
                }
                else
                {
                    needAKey = true;
                    Invoke("ResetNeedAKey", 3);
                }
            }
        }

        if (opened)
        {
            Time.timeScale = 0;
        }
    }


    private void OnGUI()
    {
        if (opened)
        {
            GUI.Box(msgRect, "You WIN!");
        }

        if (needAKey)
        {
            GUI.Box(msgRect, "You need a key!");
        }
    }



    private void ResetNeedAKey()
    {
        needAKey = false;
    }

    private void DoorOpen()
    {
        opened = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController hero = collision.gameObject.GetComponent<PlayerController>();

        if(hero != null)
        {
            this.hero = hero;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController hero = collision.gameObject.GetComponent<PlayerController>();

        if (hero != null)
        {
            this.hero = null;
        }
    }

}
