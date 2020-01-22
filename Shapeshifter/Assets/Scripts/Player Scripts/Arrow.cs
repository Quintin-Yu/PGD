using System.Collections;
using UnityEngine;

/**
 * This script is a projectile that hurts enemies
 * 
 * Code that makes the arrow stick to the wall and bounce off the melee enemy was made by Nicolaas
 * The rest was made by Bowen
 *
 * @Authors Nicolaas Schuddeboom and Bowen
 */ 

public class Arrow : Projectiles
{
    // Declare variables
    Rigidbody2D arrow;
    public float damage;

    // Initialize variables
    private void Start()
    {
        projectileLifeTime = 10;
        damage = 20f;
        arrow = GetComponent<Rigidbody2D>();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        // If the arrow hits the map...
        if (other.gameObject.tag == "map")
        {
            FindObjectOfType<AudioManager>().Play("Hit Ranged Dirt");

            // It should stop rotating
            shouldRotate = false;

            // Stop it's velocity, gravity and give this a ridiculous mass. This way, it will completely stop the rigidbody
            if (rb.velocity.x < 0)
            {
                mirrored = true;
            }

            arrow.velocity = Vector2.zero;
            arrow.gravityScale = 0;
            arrow.mass = 9999;

            // Make the arrow an object to stand on
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else
        {
            // Else run it's normal projectile code (destroying itself)
            base.OnTriggerEnter2D(other);
        }

        // If the arrow hit an ranged enemy...
        if (other.gameObject.tag == "EnemyRanged")                                                          //If collides with an ranged enemy it destroys the objet
        {
            // Play audio
            FindObjectOfType<AudioManager>().Play("Hit Ranged");

            // Get stats
            CharacterStats archerStats = other.transform.GetComponent<CharacterStats>();

            // Deal damage
            GetComponent<CombatController>().Attack(archerStats);

            // Show health bar
            other.transform.GetComponent<EnemyRanged>().healthBar.SetActive(true);
            other.transform.GetComponent<EnemyRanged>().hpTimer = 2;

            // Destroy this arrow
            Destroy(gameObject);
        }

        // Flip the x velocity if the arrow hits a melee enemy
        if (other.gameObject.tag.Equals("EnemyMelee"))                                                      //If the arrow collides with an melee enemy it bounces off the enemy
        {
            Vector2 arrowVel = arrow.velocity;
            arrowVel.x *= -.8f;
            arrow.velocity = arrowVel;
        }

        // If the arrow hit a dummy
        if (other.gameObject.tag.Equals("MeleeDummy"))                //If the arrow collides with the map or an dummy it gets destoryed without destroying anything else
        {
            // Play audio
            FindObjectOfType<AudioManager>().Play("Hit Ranged Dirt");

            // Destroy this arrow
            Destroy(gameObject);
        }
    }
}
