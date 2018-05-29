using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTrap : MonoBehaviour {


    public float damage = 10;           // Урон, наносимый ловушкой


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var trapable = collision.GetComponent<ITrapable>();

        if (trapable != null)
        {
            trapable.OnTrapEnter(this);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        var trapable = collision.GetComponent<ITrapable>();

        if (trapable != null)
        {
            trapable.OnTrapLeave(this);
        }
    }

}
