using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{ 
    public GameObject enemyArrow;

    public GameObject healthBar;

    float arrowForce = 1000f;
    float fireRate;

    public float hpTimer = 2;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        recentlyAttacked = false;
        rb = GetComponent<Rigidbody2D>();
        speed = 0.1f;
        hp = 0.1f;
        fireRate = 3f;
        maxRange = 25;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        hpTimer -= Time.deltaTime;
        if (hpTimer <= 0)
        {
            healthBar.SetActive(false);
        }

        if (!recentlyAttacked)
        {
            StartCoroutine(AttackCooldown(fireRate));
            Shoot();
            recentlyAttacked = true;
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
