﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleGuy : Enemy {

    public float shootRange = 6f;           // Расстояние с которого фиолетовый парень останавливается и начинает вести огонь картофелем
    public float angryRangeX = 12f;         // Горизонтальное расстояние на котором враг начинает двигаться к игроку
    public float angryRangeY = 1.5f;        // Вертикальное расстояние на котором враг начинает двигаться к игроку


    private bool playerInAngryRangeX;
    private bool playerInAngryRangeY;
    private bool canShoot;



    protected override void FixedUpdate()
    {
        playerInAngryRangeX = PlayerInAngryRangeX();
        playerInAngryRangeY = PlayerInAngryRangeY();
        canShoot = CanShoot();

        base.FixedUpdate();
    }



    protected override void ProcessMovement()
    {
        if ((hero != null) &&
            ((hero.transform.position - transform.position).magnitude < shootRange) &&
            playerInAngryRangeY &&
            canShoot)
        {
            weapon.Shoot();
        }
        else if(!movingToObstacle)
        {
            base.ProcessMovement();
        }
    }


    protected override void ProcessDirection()
    {
        
        // Следование за героем
        if((hero != null) && playerInAngryRangeY && playerInAngryRangeX && canShoot)
        {
            Vector2 heroPos = hero.transform.position,
                guyPos = transform.position;

            if (facingRight && heroPos.x < guyPos.x)
                Flip();
            else if (!facingRight && heroPos.x > guyPos.x)
                Flip();
        }
        else
        {
            base.ProcessDirection();
        }
    }


    /// <summary>
    /// Находится ли игрок на той же высоте
    /// </summary>
    /// <returns></returns>
    private bool PlayerInAngryRangeY()
    {
        return (hero != null) && (Mathf.Abs(hero.transform.position.y - transform.position.y) < angryRangeY);
    }

    /// <summary>
    /// Находится ли игрок "в зоне видимости"
    /// </summary>
    /// <returns></returns>
    private bool PlayerInAngryRangeX()
    {
        return (hero != null) && (Mathf.Abs(hero.transform.position.x - transform.position.x) < angryRangeX);
    }

    /// <summary>
    /// Есть ли прострел до игрока
    /// </summary>
    /// <returns></returns>
    private bool CanShoot()
    {
        return (hero != null) && !Physics2D.Linecast(
                    transform.position, 
                    hero.transform.position,
                    1 << LayerMask.NameToLayer("Limiters") |
                    1 << LayerMask.NameToLayer("ProjectileLimiters")
                );
    }
}
