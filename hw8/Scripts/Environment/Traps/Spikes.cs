using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : EnvironmentTrap
{

    public float force = 6;         // Сила с которой шипы подкидывают игрока



    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        GameObject GO = collision.gameObject;
        var trapable = collision.GetComponent<ITrapable>();

        if (trapable != null)
        {
            StartBleed(GO);
        }
    }


    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        GameObject GO = collision.gameObject;

        StopBleed(GO);
    }



    private void StartBleed(GameObject GO)
    {
        IBleedable bleedableObj = GO.GetComponent<IBleedable>();

        if (bleedableObj != null)
        {
            GameObject bloodGO = bleedableObj.GetBloodGO();
            ParticleSystem bloodPS = bleedableObj.GetBloodPS();

            var bloodPSmain = bloodPS.main;
            var bloodPSshape = bloodPS.shape;


            // Устанавливаем настройки для кровяки)
            bloodPSmain.startSpeed = 3;
            bloodPSshape.randomDirectionAmount = 0.5f;

            
            // Установим позицию кровотечения в точку соприкосновения
            bloodGO.transform.position = new Vector2(GO.transform.position.x, transform.position.y);

            // Установим угол поворота 90 градусов, т.е. вверх, относительно направления вправо
            bloodGO.transform.rotation = Quaternion.Euler(0, 0, 90);

            // Включим анимацию
            bloodPS.Play();
        }
    }

    private void StopBleed(GameObject GO)
    {
        IBleedable bleedableObj = GO.GetComponent<IBleedable>();

        if (bleedableObj != null)
        {
            bleedableObj.StopBleed();
        }
    }

}
