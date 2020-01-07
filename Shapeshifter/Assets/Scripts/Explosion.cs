using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "MeleeDummy" || collision.gameObject.tag == "EnemyMelee" || collision.gameObject.tag == "EnemyRanged")
        {
            collision.GetComponent<CharacterStats>().TakeDamage(40);
            collision.GetComponent<EnemyMelee>().healthBar.SetActive(true);
            collision.GetComponent<EnemyMelee>().hpTimer = 2;
        }
    }
}
