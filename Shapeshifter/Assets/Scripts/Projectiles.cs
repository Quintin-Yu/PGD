using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Projectiles : MonoBehaviour
{
    Rigidbody2D rb;
    public float projectileLifeTime;

    private void Update()
    {
        projectileLifeTime -= Time.deltaTime;
        rb = GetComponent<Rigidbody2D>();

        if (projectileLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.AngleAxis(Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag.Equals("map"))
        {
            Destroy(gameObject);
        }
    }
}
