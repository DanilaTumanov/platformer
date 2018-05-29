using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSaw : EnvironmentTrap, IPeriodicDamageDealer
{

    public float damagePeriod = 1f;         // Период с которым пила наносит урон

    private PeriodicDamageDriver PeriodicDamageDriver;


    private void Start()
    {
        PeriodicDamageDriver = new PeriodicDamageDriver(this);
    }



    protected void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject GO = collision.gameObject;
        IDamagable damagableObj = GO.GetComponent<IDamagable>();

        if (damagableObj != null)
        {
            switch (LayerMask.LayerToName(collision.gameObject.layer))
            {
                case "Hero":
                case "Enemies":
                    PeriodicDamageDriver.DoDamage(damagableObj);
                    break;
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        IDamagable damagableObj = collision.gameObject.GetComponent<IDamagable>();

        if (damagableObj != null)
        {
            PeriodicDamageDriver.StopDamage(damagableObj);
        }
    }



    public float GetDamage()
    {
        return damage;
    }

    public float GetDamagePeriod()
    {
        return damagePeriod;
    }
}
