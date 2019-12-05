using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "MeleeDummy" || collision.gameObject.tag == "EnemyMelee")
        {
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
