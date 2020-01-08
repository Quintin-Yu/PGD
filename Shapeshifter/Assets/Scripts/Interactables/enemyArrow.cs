using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyArrow : Projectiles
{
    private void Start()
    {
        projectileLifeTime = 10;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerGroundCollider" || other.gameObject.tag == "Shield")
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Fighter>().TakeDamage(30);
                Destroy(gameObject);
            }
            else if (other.gameObject.tag == "PlayerGroundCollider")
            {
                other.gameObject.transform.parent.GetComponent<Fighter>().TakeDamage(30);
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
        
    }
}
