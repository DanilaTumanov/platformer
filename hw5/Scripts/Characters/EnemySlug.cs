using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlug : Enemy {


    protected override void ProcessDirection()
    {
        if (playerController.isGrounded)
        {
            base.ProcessDirection();
        }
    }

}
