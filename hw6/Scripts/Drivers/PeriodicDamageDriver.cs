using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicDamageDriver {
    
    private Dictionary<int, Coroutine> PeriodicDamageCoroutines = new Dictionary<int, Coroutine>();
    private MonoBehaviour owner;
    private IPeriodicDamageDealer damagingObj;

    public PeriodicDamageDriver(MonoBehaviour owner)
    {
        this.owner = owner;
        damagingObj = owner.GetComponent<IPeriodicDamageDealer>();
    }

    public void DoDamage(IDamagable damagableObj)
    {
        // Создаем для повреждаемого объекта корутину, которая будет с определенной периодичностью его дамажить
        // И сохраняем ее в словарь под уникальным ID объекта. Таким образом, мы сможем обрабатывать периодический
        // урон сразу по нескольким объектам, а когда коллизия с одним из них прекратится, сможем легко найти и
        // остановить нужную корутину.
        PeriodicDamageCoroutines[damagableObj.GetInstanceID()] = owner.StartCoroutine(
            DoPeriodicDamage(
                damagableObj,
                damagingObj
            ));
    }


    public void StopDamage(IDamagable damagableObj)
    {
        int id = damagableObj.GetInstanceID();
        
        if (PeriodicDamageCoroutines.ContainsKey(id))
        {
            owner.GetComponent<MonoBehaviour>().StopCoroutine(PeriodicDamageCoroutines[id]);
        }
    }


    private IEnumerator DoPeriodicDamage(IDamagable damagableObj, IPeriodicDamageDealer damagingObject)
    {
        while (true)
        {
            if(damagableObj != null)
            {
                damagableObj.Damage(damagingObject);
                yield return new WaitForSeconds(damagingObject.GetDamagePeriod());
            }
            else
            {
                yield break;
            }       
        }
    }

}
