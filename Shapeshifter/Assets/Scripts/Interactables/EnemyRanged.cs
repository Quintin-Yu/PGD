using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is a child of the Enemy class.
 * It contains the movement, attack and how the enemy ranged can get damaged
 */

public class EnemyRanged : Enemy
{
    float arrowForce = 1000f;
    float fireRate;

    bool isFlipped = false;

    [Header("Needed variables/prefabs")]
    public GameObject enemyArrow;
    public Animator animator;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        recentlyAttacked = false;
        rb = GetComponent<Rigidbody2D>();
        speed = 0.1f;
        hp = 0.1f;
        fireRate = 3f;
        //maxRange = 25;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!recentlyAttacked)
        {
            StartCoroutine(AttackCooldown(fireRate));
            Shoot();
            recentlyAttacked = true;
        }

        //flipping charactersprite and healthbar depending on which side the player is from the enemy
        if (targetPlayer.transform.position.x - rb.transform.position.x >= -maxRange && targetPlayer.transform.position.x - rb.transform.position.x <= 0)
        {
            if (isFlipped == true)
            {
                healthBar.gameObject.transform.Rotate(0, 180, 0);
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                isFlipped = false;
            }
        }
        if (rb.transform.position.x - targetPlayer.transform.position.x >= -maxRange && rb.transform.position.x - targetPlayer.transform.position.x <= 0)
        {
            if (isFlipped == false)
            {
                healthBar.gameObject.transform.Rotate(0, 180, 0);
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                isFlipped = true;
            }
        }
    }

    public void Shoot()
    {
        if (targetPlayer.transform.position.x - rb.transform.position.x >= -maxRange && targetPlayer.transform.position.x - rb.transform.position.x <= 0)
        {
            StartCoroutine(shootDelay(1));   
        }
        else if (rb.transform.position.x - targetPlayer.transform.position.x >= -maxRange && rb.transform.position.x - targetPlayer.transform.position.x <= 0)
        {
            StartCoroutine(shootDelay(1));
        }
    }

    //fire an arrow after a 1 second charge delay with the appropriate direction, also applies animation
    IEnumerator shootDelay(float time)
    {
        animator.SetBool("isShooting", true);
        yield return new WaitForSeconds(time);
        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized * arrowForce;
        direction.Normalize();
        GameObject newArrow = Instantiate(enemyArrow, transform.position, Quaternion.identity);
        newArrow.transform.position += direction * 0.5f;
        newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);
        animator.SetBool("isShooting", false);
    }

    

}
