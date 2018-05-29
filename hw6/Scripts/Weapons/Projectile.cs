using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageDealer {

    public float speed = 1000f;
    public float damage = 20f;

    protected Rigidbody2D rb;
    
    // Use this for initialization
    protected virtual void Start () {
        Vector2 forceDirection = transform.right;
        forceDirection.Normalize();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(forceDirection * speed);
	}
	

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        var damagableObj = collision.gameObject.GetComponent<IDamagable>();

        if (damagableObj != null) {
            damagableObj.Damage(this);
        }        
    }


    public float GetDamage()
    {
        return damage;
    }

}
