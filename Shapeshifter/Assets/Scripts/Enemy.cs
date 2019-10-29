using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public GroundCollider groundCollider;
    public GameObject player;

    float speed = 0.1f;
    int jumpHeight;
    int hp = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    private void FixedUpdate ()
    {
        if (player.transform.position.x - rb.transform.position.x >= -40 && player.transform.position.x - rb.transform.position.x <= 0)
        {
            transform.Translate(-speed, 0f, 0f);
        }

        if (rb.transform.position.x - player.transform.position.x >= -40 && rb.transform.position.x - player.transform.position.x <= 0)
        {
            transform.Translate(speed, 0f, 0f);
        }
    }
}
