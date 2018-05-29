using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, IMovable, IDamagable, IDamageDealer, IPeriodicDamageDealer
{

    public float speed = 5f;
    public float HP = 100f;
    public float collisionDamage = 25f;                  // Урон, который наносит враг при столкновении с игроком
    public float collisionDamagePeriod = 1f;
    public bool passive = false;
    public PickupDropSettings[] DropablePickups;

    protected bool facingRight = true;
    protected GameObject hero;
    protected PlayerController playerController;
    protected Weapon weapon;
    protected Rigidbody2D rb;
    protected Vector2 velocity = Vector2.zero;
    protected Transform frontCheck;
    protected bool movingToObstacle = false;
    protected PeriodicDamageDriver PeriodicDamageDriver;
    protected PickupDropDriver PickupDropDriver;

    // Use this for initialization
    void Start () {
        hero = GameObject.FindGameObjectWithTag("Hero");
        playerController = hero.GetComponent<PlayerController>();
        weapon = gameObject.GetComponentInChildren<Weapon>();
        rb = GetComponent<Rigidbody2D>();
        frontCheck = transform.Find("frontCheck");
        PeriodicDamageDriver = new PeriodicDamageDriver(this);
        PickupDropDriver = new PickupDropDriver(DropablePickups);

        if (Random.Range(0, 2) == 0)
            Flip();
        
        StartCoroutine(CheckObstacle());
    }



    protected virtual void FixedUpdate () {

        ProcessDirection();
        ProcessMovement();        

	}


    protected virtual void ProcessMovement()
    {
        if (!passive)
        {
            velocity.y = rb.velocity.y;
            velocity.x = speed;
            rb.velocity = velocity;
        }
    }

    protected virtual void ProcessDirection()
    {
        if (movingToObstacle)
        {
            Flip();
            movingToObstacle = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject GO = collision.gameObject;
        IDamagable damagableObj = GO.GetComponent<IDamagable>();
        
        if (damagableObj != null)
        {
            switch (LayerMask.LayerToName(GO.layer))
            {
                case "Hero":
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
    


    protected void Flip ()
    {
        // Меняем флаг направления на противоположный
        facingRight = !facingRight;

        // Меняем масштаб по оси x на число с противоположным знаком
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        // Изменим направление скорости на противоположное. Так будет лучше. чем умножать ее каждый раз в update
        speed *= -1;
    }


    public void Hurt(float dmg)
    {
        HP -= dmg;

        if (HP <= 0)
            Die();
    }

    private void Die()
    {
        // Активируем звук смерти
        GetComponent<AudioSource>().Play();

        // Выкидываем дроп
        PickupDropDriver.DropPickup(transform.position);

        // Уничтожаем объект через 200ms, чтобы успел сработать звук
        Destroy(gameObject, 0.2f);
    }

    private IEnumerator CheckObstacle()
    {
        while (true)
        {
            /*bool hasObstacle = Physics2D.Raycast(
                new Vector2(transform.position.x, transform.position.y),
                transform.right * (facingRight ? 1 : -1),
                1,
                1 << LayerMask.NameToLayer("Limiters"));*/

            movingToObstacle = Physics2D.Linecast(
                transform.position,
                frontCheck.position,
                1 << LayerMask.NameToLayer("Limiters") |
                1 << LayerMask.NameToLayer("EnemyLimiters"));
            

            // Чем быстрее движется враг - тем чаще запускаем корутину
            yield return new WaitForSeconds(0.5f / speed);
        }
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public void Damage(IDamageDealer damageDealer)
    {
        Hurt(damageDealer.GetDamage());
    }

    public float GetDamage()
    {
        return collisionDamage;
    }

    public float GetDamagePeriod()
    {
        return collisionDamagePeriod;
    }
}

