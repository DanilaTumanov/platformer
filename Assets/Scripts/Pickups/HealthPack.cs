using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Pickup
{

    public float healing = 0;       // Сила хила аптечки



    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        IHealable healableObj = collision.gameObject.GetComponent<IHealable>();

        if(healableObj != null)
        {
            healableObj.Heal(healing);

            base.OnCollisionEnter2D(collision);
        }        
    }
}
