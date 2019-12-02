using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{ 
    public GameObject enemyArrow;

    float arrowForce = 1000f;
    float fireRate;
    float nextFire;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        speed = 0.1f;
        hp = 0.1f;
        fireRate = 3f;
        nextFire = 0.0f;
        maxRange = 25;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (Time.time > nextFire)
        {
            Shoot();
            nextFire = Time.time + fireRate;
        }   
    }

    public void Shoot()
    {
        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized * arrowForce;
        direction.Normalize();

        if (targetPlayer.transform.position.x - rb.transform.position.x >= -maxRange && targetPlayer.transform.position.x - rb.transform.position.x <= 0)
        {
            GameObject newArrow = Instantiate(enemyArrow, transform.position, Quaternion.identity);
            newArrow.transform.position += direction * 0.5f;
            newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);

        }

        else if (rb.transform.position.x - targetPlayer.transform.position.x >= -maxRange && rb.transform.position.x - targetPlayer.transform.position.x <= 0)
        {
            GameObject newArrow = Instantiate(enemyArrow, transform.position, Quaternion.identity);
            newArrow.transform.position += direction * 0.5f;
            newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);
        }
    }
}
