using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStats : MonoBehaviour {


    PlayerController hero;

    Image healthBar;
    Text healthCount;
    ParticleSystem healthDecreaseAnimatePS;
    ParticleSystem healthIncreaseAnimatePS;

    Vector2 newSizeDelta;
    bool HPdecreasing = false;
    bool HPincreasing = false;

    // Use this for initialization
    void Start () {
        hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<PlayerController>();

        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        healthCount = GameObject.Find("HealthCount").GetComponent<Text>();
        healthDecreaseAnimatePS = GameObject.Find("HealthDecreaseAnimate").GetComponent<ParticleSystem>();
        healthIncreaseAnimatePS = GameObject.Find("HealthIncreaseAnimate").GetComponent<ParticleSystem>();

        newSizeDelta = healthBar.rectTransform.sizeDelta;


        hero.OnHurt += Hurted;
        hero.OnHeal += Healed;
    }
	
	// Update is called once per frame
	void Update () {

        ProcessHealthBar();

	}



    private void ProcessHealthBar()
    {
        if (HPdecreasing || HPincreasing)
        {
            newSizeDelta.x = Mathf.Lerp(newSizeDelta.x, hero.HP / 2, 0.2f);
            
            if (healthBar.rectTransform.sizeDelta.x <= newSizeDelta.x + 1)
            {
                healthDecreaseAnimatePS.Stop();
                HPdecreasing = false;
            }
            if (healthBar.rectTransform.sizeDelta.x >= newSizeDelta.x - 1)
            {
                healthIncreaseAnimatePS.Stop();
                HPincreasing = false;
            }


            healthBar.rectTransform.sizeDelta = newSizeDelta;

            if (hero.HP > 150)
                healthCount.text = (int)hero.HP + " HP";
            else
                healthCount.text = String.Empty;
        }        
    }



    public void Hurted()
    {
        HPdecreasing = true;
        healthDecreaseAnimatePS.Play();
    }


    public void Healed()
    {
        HPincreasing = true;
        healthIncreaseAnimatePS.Play();
    }
}
