using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSaw : EnvironmentTrap, IPeriodicDamageDealer
{

    public float damagePeriod = 1f;         // Период с которым пила наносит урон
    public bool clockWise = true;           // Пила вращается по часовой стрелке

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

        StartBleed(GO, collision);
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject GO = collision.gameObject;
        IDamagable damagableObj = collision.gameObject.GetComponent<IDamagable>();

        if (damagableObj != null)
        {
            PeriodicDamageDriver.StopDamage(damagableObj);
        }

        StopBleed(GO, collision);
    }



    public float GetDamage()
    {
        return damage;
    }

    public float GetDamagePeriod()
    {
        return damagePeriod;
    }



    private void StartBleed(GameObject GO, Collision2D collision)
    {
        IBleedable bleedableObj = GO.GetComponent<IBleedable>();

        if(bleedableObj != null)
        {
            GameObject bloodGO = bleedableObj.GetBloodGO();
            ParticleSystem bloodPS = bleedableObj.GetBloodPS();
            ContactPoint2D[] contacts = new ContactPoint2D[1];

            var bloodPSmain = bloodPS.main;
            var bloodPSshape = bloodPS.shape;


            // Устанавливаем настройки для кровяки)
            bloodPSmain.startSpeed = 7;
            bloodPSshape.randomDirectionAmount = 0.15f;

            // Получим точки соприкосновения коллайдеров
            collision.GetContacts(contacts);

            // Установим позицию кровотечения в точку соприкосновения
            bloodGO.transform.position = contacts[0].point;

            // Установим угол поворота перпендикулярным к нормали, для этого воспользуемся умножением векторов
            bloodGO.transform.rotation = Quaternion.FromToRotation(Vector3.right, Vector3.Cross(Vector3.forward, contacts[0].normal));
            
            // Включим анимацию
            bloodPS.Play();
        }
    }

    private void StopBleed(GameObject GO, Collision2D collision)
    {
        IBleedable bleedableObj = GO.GetComponent<IBleedable>();

        if (bleedableObj != null)
        {
            bleedableObj.StopBleed();
        }
    }
}
