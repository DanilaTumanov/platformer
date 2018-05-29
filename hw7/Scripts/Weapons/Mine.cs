using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Trap {

    public float range = 10;
    public float force = 30;
    public float depth = 1f;      // Глубина виртуального "зарытия" мины. Делает точку начала взрыва ниже, чтобы враги сбоку немного подлетали
    
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var GO = collision.gameObject;
        if (GO.tag == "Enemy")
        {
            Explode();
        }
    }

    public void Explode()
    {
        // Сделаем кэш позиции мины, чтобы не доставать его каждый раз
        var minePos = transform.position;

        // Моделируем взрыв из глубины depth вверх

        RaycastHit2D[] EnemiesInExplodeRange = Physics2D.CircleCastAll(
            new Vector2(minePos.x, minePos.y - depth),    // Начинаем с текущих координат, опущенных на depth
            range,                                                              // Радиусом range
            Vector2.up,                                                         // По направлению вверх
            depth,                                                              // На расстояние равное глубине, т.е. до фактического положения мины
            1 << LayerMask.NameToLayer("Enemies") |
            1 << LayerMask.NameToLayer("Hero")
        );
        int length = EnemiesInExplodeRange.GetLength(0);

        // Для всех объектов, попавших в зону поражения
        for (int i = 0; i < length; i++)
        {
            // найдем нормаль к поверхности
            Vector2 normal = EnemiesInExplodeRange[i].normal;
            normal.Normalize(); // Приведем к единичному вектору

            // найдем модификатор расстояния (чем дальше объект от эпицентра - тем меньше урон и импульс)
            float distMod = (range - Vector2.Distance(minePos, EnemiesInExplodeRange[i].transform.position)) / range;

            // Умножаем на -силу, т.к. нормаль направлена в центр круга
            Vector3 forceVector = normal * -force * distMod;
            
            // Добавим импульс объекту в направлении, противоположном нормали
            EnemiesInExplodeRange[i].collider.GetComponent<Rigidbody2D>().AddForce(forceVector, ForceMode2D.Impulse);

            // Попробуем получить класс Enemy
            Enemy enemy = EnemiesInExplodeRange[i].collider.GetComponent<Enemy>();
            // Если такой класс есть, значит это враг, в таком случае...
            if (enemy != null)
            {
                // ...Вызовем метод, наносящий урон врагу
                enemy.Hurt(damage * distMod);
            }
        }

        // Удаляем мину
        Destroy(gameObject);
    }
}
