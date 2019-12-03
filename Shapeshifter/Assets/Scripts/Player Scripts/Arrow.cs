using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D arrow;              //Get's own rigidbody

    private void Start()
    {
        arrow = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "EnemyRanged")                                                          //If collides with an ranged enemy it destroys the objet
        {
            Debug.Log("hit");
            FindObjectOfType<AudioManager>().Play("Hit Ranged");
            Destroy(other.transform.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag.Equals("EnemyMelee"))                                                      //If the arrow collides with an melee enemy it bounces off the enemy
        {
            Vector2 arrowVel = arrow.velocity;
            arrowVel.x *= -1;
            arrow.velocity = arrowVel;
        }

        if (other.gameObject.tag.Equals("map") || other.gameObject.tag.Equals("MeleeDummy"))                //If the arrow collides with the map or an dummy it gets destoryed without destroying anything else
        {
            FindObjectOfType<AudioManager>().Play("Hit Ranged Dirt");
            Destroy(gameObject);
        }
    }
}
