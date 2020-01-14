using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is a parent class for all the projectiles.
 * 
 * @author Quintin Yu
 */

[SerializeField]
public class Projectiles : MonoBehaviour
{
    public float projectileLifeTime;
    public Rigidbody2D rb;

    public bool shouldRotate;

    private void Awake()
    {
        shouldRotate = true;
    }

    private void Update()
    {
        Destroy(gameObject, projectileLifeTime);                    //Destroys the projectile after a set amount of time.
    }

    private void FixedUpdate()
    {
        if (shouldRotate)                                           //Checks if a projectile is supposed to rotate (such as the arrow to go in an arc)
        {
            float rad = Mathf.Atan(rb.velocity.y / rb.velocity.x);
            float deg = rad * 180 / Mathf.PI;
            Debug.Log(deg);
            transform.eulerAngles = new Vector3(0, 0, deg);
        }
        else
        {
            rb.velocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)          //If a projectile hits the map it get's destroyed.
    {
        if (other.gameObject.tag.Equals("map"))
        {
            Destroy(gameObject);
        }
    }
}
