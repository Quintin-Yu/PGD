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
        
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
