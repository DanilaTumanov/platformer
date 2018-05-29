using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupDropDriver {

    private PickupDropSettings[] pickups;       // Настройки всех выпадающих предметов
    private int accuracy = 100;                 // Точность рассчета вероятности (до сотых долей по-умолчанию)


    public PickupDropDriver(PickupDropSettings[] pickups)
    {
        this.pickups = pickups;
    }

    public PickupDropDriver(PickupDropSettings[] pickups, int accuracy) : this(pickups)
    {
        this.accuracy = accuracy;
    }


    public void DropPickup(Vector2 position)
    {
        // Проходимся по всем настройкам
        for(var i = 0; i < pickups.Length; i++)
        {
            // Генерируем число от 0 до 100 с заданной точностью. Если число попало в указанный процент, 
            // то создаем предмет и выходим из цикла. Иначе продолжаем дальше.
            if(((float) Random.Range(0, 100 * accuracy) / accuracy) < pickups[i].possibility)
            {
                Object.Instantiate(pickups[i].pickup, position, Quaternion.identity);
                break;
            }

            // При таком подходе будет учитываться не чистая вероятность, а вероятность с учетом уже невыпавших предметов.
            // К примеру, у нас есть 3 предмета, каждый из которых выпадает с вероятностью 30, 40 и 50% соответственно.
            // Первый предмет выпадет со своей чистой вероятностью - 30%.
            // Второй предмет выпадет только если не выпал первый, а вероятность этого 70%. Значит приведенная вероятность
            // выпадения второго предмета будет 40% от 70%, т.е. 70 * 0.4 = 28%
            // Вероятность выпадения 3его предмета так же будет зависеть от невыпадения первых двух предметов, 
            // а вероятность этого равна 70% * 0.6 = 42%. Значит приведенная вероятность выпадения 3 предмета будет 50 * 0.42 = 21%
        }
    }

}
