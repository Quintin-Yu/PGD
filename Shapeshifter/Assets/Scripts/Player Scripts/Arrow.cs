using System.Collections;
using UnityEngine;

public class Arrow : Projectiles
{
    Rigidbody2D arrow;              //Get's own rigidbody
    public float damage;

    private void Start()
    {
        projectileLifeTime = 10;
        damage = 20f;
        arrow = GetComponent<Rigidbody2D>();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        //base.OnTriggerEnter2D(other);

        if (other.gameObject.tag == "map")
        {
            shouldRotate = false;

            arrow.velocity = Vector2.zero;
            arrow.gravityScale = 0;
            arrow.mass = 9999;

            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else
        {
            base.OnTriggerEnter2D(other);
        }

        if (other.gameObject.tag == "EnemyRanged")                                                          //If collides with an ranged enemy it destroys the objet
        {
            FindObjectOfType<AudioManager>().Play("Hit Ranged");

            CharacterStats archerStats = other.transform.GetComponent<CharacterStats>();
            GetComponent<CombatController>().Attack(archerStats);

            other.transform.GetComponent<EnemyRanged>().healthBar.SetActive(true);
            other.transform.GetComponent<EnemyRanged>().hpTimer = 2;
            Destroy(gameObject);
        }

        if (other.gameObject.tag.Equals("EnemyMelee"))                                                      //If the arrow collides with an melee enemy it bounces off the enemy
        {
            Vector2 arrowVel = arrow.velocity;
            arrowVel.x *= -.8f;
            arrow.velocity = arrowVel;
        }

        if (other.gameObject.tag.Equals("MeleeDummy"))                //If the arrow collides with the map or an dummy it gets destoryed without destroying anything else
        {
            FindObjectOfType<AudioManager>().Play("Hit Ranged Dirt");
            Destroy(gameObject);
        }
    }
}
