using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleGuy : Enemy {

    public float shootRange = 6f;           // Расстояние с которого фиолетовый парень останавливается и начинает вести огонь картофелем
    public float angryRangeX = 12f;         // Горизонтальное расстояние на котором враг начинает двигаться к игроку
    public float angryRangeY = 1.5f;        // Вертикальное расстояние на котором враг начинает двигаться к игроку
    public float fireRatePC = 0.1f;
    public float fireRateAndroid = 3f;


    private bool playerInShootingRange;
    private bool playerInAngryRangeX;
    private bool playerInAngryRangeY;
    private bool canShoot;
    private bool shooting;                  // Ведет ли огонь


    private bool cheat = false;             // Чит для легкого прохождения


    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            cheat = !cheat;
        }
    }


    protected override void FixedUpdate()
    {
        playerInAngryRangeX = PlayerInAngryRangeX();
        playerInAngryRangeY = PlayerInAngryRangeY();
        canShoot = CanShoot();
        playerInShootingRange = PlayerInShootingRange();

        base.FixedUpdate();

        ProcessFire();
    }

    private void ProcessFire()
    {
        if (canShoot &&
            playerInAngryRangeY &&
            playerInShootingRange &&
            !shooting
            && !cheat)
        {
#if UNITY_ANDROID
            InvokeRepeating("Fire", 0, fireRateAndroid);
#else
            InvokeRepeating("Fire", 0, fireRatePC);
#endif
            shooting = true;
        }
        else if(!canShoot)
        {
            CancelInvoke("Fire");
            shooting = false;
        }
    }


    private void Fire()
    {
        if (weapon.Shoot())
        {
            animator.SetTrigger("Fire");
        }
    }


    protected override void ProcessMovement()
    {
        
        if (!movingToObstacle &&
            !(playerInShootingRange &&
            playerInAngryRangeY &&
            canShoot))
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
                    1 << LayerMask.NameToLayer("ProjectileLimiters") |
                    1 << LayerMask.NameToLayer("Ground")
                );
    }


    private bool PlayerInShootingRange()
    {
        return (hero.transform.position - transform.position).magnitude < shootRange;
    }


    protected override void ProcessAnimation()
    {
        animator.SetBool("ReadyToShoot", canShoot && playerInShootingRange && playerInAngryRangeY);
    }
}
