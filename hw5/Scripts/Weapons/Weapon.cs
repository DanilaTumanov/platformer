using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Projectile projectile;
    public float cooldown = 1;

    private Transform barrel;
    private IMovable owner;
    private Vector3 vector0z = new Vector3(0, 0, 0);
    private Vector3 vector180z = new Vector3(0, 0, 180f);
    protected float lastShotTime;                               // Время последнего выстрела. Нужно для контроля скорострельности оружия

    // Use this for initialization
    void Start () {
        barrel = transform.Find("Barrel");
        owner = transform.root.GetComponent<IMovable>();
        lastShotTime = Time.time - cooldown;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot()
    {
        if (Time.time > lastShotTime + cooldown)
        {
            // Определяем модификатор поворота снаряда в зависимости от того, куда смотрит игрок.
            Vector3 facingMod = owner.IsFacingRight() ? vector0z : vector180z;
            
            // Создаем снаряд в координатах ствола (barrel) и с нужным поворотом - поворот снаряда, плюс поворот оружия плюс модификатор
            var proj = Instantiate<Projectile>(projectile, barrel.position, projectile.transform.rotation * transform.rotation * Quaternion.Euler(facingMod));

            // Дааа... тут криво, конечно.. Нужно выбрать в какой слой положить снаряд, чтобы нормально обрабатывались столкновения
            // Пока не придумал, как сделать это хорошо, да и систему оружия придется подкручивать, когда доделаю выпадающее оружие
            switch(LayerMask.LayerToName(gameObject.layer)){
                case "Hero":
                    proj.gameObject.layer = LayerMask.NameToLayer("Projectiles");
                    break;
                case "Enemies":
                    proj.gameObject.layer = LayerMask.NameToLayer("EnemyProjectiles");
                    break;
            }

            // Активируем звук выстрела
            GetComponent<AudioSource>().Play();

            lastShotTime = Time.time;
        }        
    }

}
