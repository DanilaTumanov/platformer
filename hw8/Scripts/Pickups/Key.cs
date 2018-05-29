using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickup {

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController Hero = collision.gameObject.GetComponent<PlayerController>();

        if (Hero != null)
        {
            Hero.SetKey();

            base.OnCollisionEnter2D(collision);
        }
    }
}
