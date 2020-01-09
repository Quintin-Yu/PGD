using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Destroy(gameObject, projectileLifeTime);
    }

    private void FixedUpdate()
    {
        Debug.Log(shouldRotate);

        if (shouldRotate)
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

    public virtual void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag.Equals("map"))
        {
            Destroy(gameObject);
        }
    }
}
