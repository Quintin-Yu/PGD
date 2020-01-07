using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Projectiles : MonoBehaviour
{
    public float projectileLifeTime;
    public Rigidbody2D rb;

    private void Update()
    {
        Destroy(gameObject, projectileLifeTime);
    }

    private void FixedUpdate()
    {
        float rad = Mathf.Atan(rb.velocity.y / rb.velocity.x);
        float deg = rad * 180 / Mathf.PI;
        transform.eulerAngles = new Vector3(0, 0, deg);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag.Equals("map"))
        {
            Destroy(gameObject);
        }
    }
}
