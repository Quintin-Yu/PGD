using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    bool canDoDamage = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "MeleeDummy" || collision.gameObject.tag == "EnemyMelee" || collision.gameObject.tag == "EnemyRanged")
        {
            int layerMask = 1 << 9;
            layerMask = ~layerMask;
            Vector2 enemyDirection = (collision.transform.position - transform.position).normalized;
            RaycastHit2D enemyHit = Physics2D.Raycast(transform.position, enemyDirection, 15, layerMask);

            if (enemyHit.collider != null)
            {
                Debug.Log(enemyHit.collider.name);
                Debug.DrawRay(transform.position, enemyDirection * 15);

                if (enemyHit.collider.tag.Equals("map"))
                {
                    canDoDamage = false;
                }
            }


            if (canDoDamage)
            {
                collision.GetComponent<CharacterStats>().TakeDamage(40);
                collision.GetComponent<EnemyMelee>().healthBar.SetActive(true);
                collision.GetComponent<EnemyMelee>().hpTimer = 2;
            }
        }
    }
}