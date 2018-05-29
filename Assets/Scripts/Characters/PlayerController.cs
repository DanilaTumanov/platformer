using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IMovable, IClimbable, ITrapable, IDamagable, IHealable
{

    public float HP = 500;              // Здоровье игрока
    public float MaxHP = 500;           // Максимальное здоровье игрока
    public float speed = 5;             // Скорость передвижения игрока
    public float jumpForce = 10;        // Сила прыжка
    public float climbingSpeed = 3;     // Скорость вертикального передвижения (например, по лестницам)
    public Trap trap;                   // Префаб ловушки
    public int trapNumber = 5;          // Количество ловушек

    [HideInInspector]
    public bool facingRight = true;     // Смотрит ли игрок направо
    [HideInInspector]
    public bool isGrounded = true;      // Находится ли герой на земле
    [HideInInspector]
    public bool wasGrounded = true;     // Находился ли герой на земле в прошлом кадре

    private Vector2 jumpVector;         // Вектор для указания силы прыжка
    private bool canJump = false;       // Может ли игрок совершить прыжок
    private Transform groundCheck;      // Объект для проверки нахождения на земле
    private Weapon Weapon;              // Скрипт оружия
    private Transform weapon;           // Объект оружия, которое должно быть потомком Hero и называться Weapon
    private Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero;
    private Ladder ladder;              // Лестница по которой забирается игрок. Нужно для обработки поведения на лестнице
    private bool climbing = false;      // Флаг, означающий, что игрок поднимается по лестнице
    private Animator animator;
    private bool hasKey;                // Найден ли ключ
    GameController Game;                // Контроллер игры, нужен для того, чтобы отслеживать состояния игры, такие как пауза.
                                        // Вообще тут нужен синглтон, но нет времени)))

    InputManager InputMgr;


    private bool cheat = false;



    public event Action OnHurt;
    public event Action OnHeal;
    public event Action OnDeath;

    // Use this for initialization
    void Start () {
        jumpVector = new Vector2(0f, jumpForce);
        groundCheck = transform.Find("groundCheck");
        weapon = transform.Find("Weapon");
        Weapon = weapon.GetComponentInChildren<Weapon>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Game = GameObject.Find("GameController").GetComponent<GameController>();
        
        InputMgr = GameObject.FindGameObjectWithTag("GameUI").GetComponent<InputManager>();
    }
    

    // Update is called once per frame
    void Update () {

        if (!Game.Paused)
        {
            GroundCheck();

            ProcessMovement();

            ProcessJump();

#if UNITY_ANDROID
            ProcessDirection();        
#else
            ProcessMouseControl();
#endif
            ProcessPrimaryWeapon();

            ProcessTraps();

            if (Input.GetButtonDown("Fire2"))
            {
                cheat = !cheat;
            }
        }
        
    }
    

    private void LateUpdate()
    {
        ProcessAnimation();
    }



    public void OnGUI()
    {
        // ОТЛАДОЧНАЯ ИНФОРМАЦИЯ
        if(cheat)
            GUI.Box(new Rect(Screen.width - 40, 0, 40, 20), "Cheat");
    }






    private void ProcessDirection()
    {
        if((facingRight && InputMgr.HorizontalAxis < 0) || (!facingRight && InputMgr.HorizontalAxis > 0))
        {
            Flip();
        }
    }


    private void ProcessAnimation()
    {

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsClimbing", climbing);

        if (isGrounded)
        {
            float h = InputMgr.HorizontalAxis;

            if (h == 0)
            {
                animator.SetBool("MovingForward", false);
                animator.SetBool("MovingBackward", false);
                animator.speed = 1;
            }
            else
            {

                bool movingFwd = (h > 0 && facingRight) || (h < 0 && !facingRight);
                animator.SetBool("MovingForward", movingFwd);
                animator.SetBool("MovingBackward", !movingFwd);
                animator.speed = 0.5f + (h >= 0 ? h : -h);
            }
        }

        if (climbing && animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbingLadder"))
        {
            float v = InputMgr.VerticalAxis;
            animator.speed = v >= 0 ? v : -v;
        }

        if (!isGrounded && wasGrounded)
        {
            animator.SetTrigger("StartLevitate");
        }

    }



    /// <summary>
    /// Обработка движения персонажа
    /// </summary>
    private void ProcessMovement()
    {

        // Получаем значение оси Horizontal
        float h = InputMgr.HorizontalAxis;
        float v = InputMgr.VerticalAxis;

        // Устанавливаем вычисленную скорость игрока
        velocity.x = h * speed;
        velocity.y = rb.velocity.y;

        // Обработка нахождения на лестнице
        if (ladder != null)
        {
            if (climbing)
            {
                velocity.y = v * climbingSpeed;
                velocity.x = 0;
                if((transform.position.y < ladder.bottom.position.y) && isGrounded)
                {
                    DisableClimbingMode();
                }
            }
            else if ((v > 0 && transform.position.y < ladder.transform.position.y) ||
                     (v < 0 && transform.position.y > ladder.transform.position.y))
            {
                EnableClimbingMode();
                transform.position = new Vector2(ladder.transform.position.x, transform.position.y);
                velocity.x = 0;
            }
        }

        // Меняем скорость игрока
        rb.velocity = velocity;
    }
    

    private void EnableClimbingMode()
    {
        climbing = true;
        rb.isKinematic = true;
        animator.SetTrigger("StartClimbing");
    }

    private void DisableClimbingMode()
    {
        climbing = false;
        rb.isKinematic = false;
    }


    /// <summary>
    /// Обработка прыжка
    /// </summary>
    private void ProcessJump()
    {
        // Если игрок находится на земле - значит можно прыгать
        canJump = isGrounded;
        
        bool jump = InputMgr.Jump;

        // Обработка нажатия кнопки прыжка
        if (jump)
        {
            if (canJump)
            {
                var v = rb.velocity;
                v.y = 0;
                rb.velocity = v;


                var jumpVector = this.jumpVector;
                if (cheat)
                    jumpVector *= 1.3f;


                rb.AddForce(jumpVector, ForceMode2D.Impulse);

                // Пока игрок находится в прыжке, прыгать нельзя
                canJump = false;
            }
            // Если находимся в режиме вертикального перемещения и нажали прыжок, то спрыгиваем с лестницы
            else if (climbing)
            {
                DisableClimbingMode();
            }
        }
    }


    /// <summary>
    /// Обработка управления мышью
    /// </summary>
    private void ProcessMouseControl()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseWorldPosition.z = weapon.transform.position.z;

        // Меняем ориентацию персонажа в зависимости от направления движения
        if (facingRight && mouseWorldPosition.x - transform.position.x < 0)
            Flip();
        else if (!facingRight && mouseWorldPosition.x - transform.position.x > 0)
            Flip();

        // Ищем угол между направлением вниз и вектором, соединяющим позицию оружия и курсора.
        float angle = Vector3.Angle(Vector2.down, mouseWorldPosition - weapon.position);

        // Поворачиваем оружие на результирующий угол - 90 градусов, т.к. в повороте по оси Z 0 находится на оси X.
        // Не забваем про то, что когда скейл отрицательный (персонаж смотрит влево), нужно умножить результат на -1,
        // Потому что угол поворота учитывает отрицательный скейл.
        weapon.rotation = Quaternion.Euler(0, 0, (angle - 90) * (facingRight ? 1 : -1));
    }
    

    /// <summary>
    /// Обработка действий первичного оружия
    /// </summary>
    private void ProcessPrimaryWeapon()
    {
        if (InputMgr.Shoot && !climbing)
        {
            Weapon.Shoot();
        }
    }

    
    /// <summary>
    /// Обработка расстановки ловушек
    /// </summary>
    private void ProcessTraps()
    {
        if (InputMgr.PlaceTrap)
        {
            PlaceTrap();
        }
    }


    /// <summary>
    /// Установка ловушки
    /// </summary>
    private void PlaceTrap()
    {
        if (trap != null && trapNumber > 0)
        {
            Instantiate<Trap>(trap, transform.position, transform.rotation);
            trapNumber--;
        }
    }


    /// <summary>
    /// Поворот персонажа в противоположную сторону
    /// </summary>
    private void Flip()
    {
        // Меняем флаг направления на противоположный
        facingRight = !facingRight;

        // Меняем масштаб по оси x на число с противоположным знаком
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
 
    
    public void Hurt(float damage)
    {
        if (cheat)
            damage /= 2;

        HP -= damage;

#if UNITY_ANDROID
        Handheld.Vibrate();
#endif

        if(OnHurt != null)
            OnHurt.Invoke();

        if (HP <= 0)
        {
            HP = 0;
            Die();
        }
    }


    private void Die()
    {
        if (OnDeath != null)
            OnDeath.Invoke();
    }


    private void GroundCheck()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.Linecast(transform.position, 
                groundCheck.position, 
                1 << LayerMask.NameToLayer("Ground")
            );
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public void SetLadder(Ladder ladder)
    {
        this.ladder = ladder;
    }

    public void RemoveLadder()
    {
        this.ladder = null;
        DisableClimbingMode();
    }

    public void OnTrapEnter(EnvironmentTrap trap)
    {
        if (trap.CompareTag("TrapSpikes"))
        {
            Hurt(trap.damage);
            rb.velocity = new Vector2(velocity.x, 0f);
            rb.AddForce(new Vector2(0f, trap.GetComponent<Spikes>().force), ForceMode2D.Impulse);
        }
    }

    public void OnTrapLeave(EnvironmentTrap trap)
    {
        
    }

    public void Damage(IDamageDealer damageDealer)
    {
        Hurt(damageDealer.GetDamage());
    }

    public void Heal(float healing)
    {
        HP += healing;

        HP = HP > MaxHP ? MaxHP : HP;

        OnHeal.Invoke();
    }


    public void SetKey()
    {
        hasKey = true;
    }

    public bool HasKey()
    {
        return hasKey;
    }
}
