using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    Rigidbody2D arrow;

    private void Start()
    {
        arrow = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("hit");
            FindObjectOfType<AudioManager>().Play("Hit Ranged");
            Destroy(other.transform.parent.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag.Equals("EnemyMelee"))
        {
            Vector2 arrowVel = arrow.velocity;
            arrowVel.x *= -1;
            arrow.velocity = arrowVel;
        }

        if (other.gameObject.tag.Equals("map") || other.gameObject.tag.Equals("MeleeDummy"))
        {
            Destroy(gameObject);
        }
    }
}
