using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is a parent class for all the projectiles.
 * Arrow rotation was done by Nicolaas
 * 
 * @authors Quintin Yu and Nicolaas Schuddeboom
 */

[SerializeField]
public class Projectiles : MonoBehaviour
{
    // Declare variables
    public float projectileLifeTime;
    public Rigidbody2D rb;

    public bool shouldRotate;

    // When the object is created, initialize shouldRotate
    private void Awake()
    {
        shouldRotate = true;
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        //Destroys the projectile after a set amount of time.
        Destroy(gameObject, projectileLifeTime);
    }

    private void FixedUpdate()
    {
        //Checks if a projectile is supposed to rotate (such as the arrow to go in an arc)
        if (shouldRotate)                                           
        {
            // If so, set rotation based on velocity
            float rad = Mathf.Atan(rb.velocity.y / rb.velocity.x);
            float deg = rad * 180 / Mathf.PI;
            transform.eulerAngles = new Vector3(0, 0, deg);
        }
        else
        {
            // Else stop it's movement completely
            rb.velocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
        }
    }

    //If a projectile hits the map it get's destroyed.
    public virtual void OnTriggerEnter2D(Collider2D other)          
    {
        if (other.gameObject.tag.Equals("map") || other.gameObject.tag.Equals("Breakable"))
        {
            Destroy(gameObject);
        }
    }
}
