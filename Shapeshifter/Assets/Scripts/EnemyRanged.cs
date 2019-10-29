using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public Rigidbody2D rb;
    public GroundCollider groundCollider;
    public GameObject player;

    float speed;
    int jumpHeight;
    float hp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        speed = 0.1f;
        hp = 0.1f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (player.transform.position.x - rb.transform.position.x >= -20 && player.transform.position.x - rb.transform.position.x <= 0)
        {
            
        }

        if (rb.transform.position.x - player.transform.position.x >= -20 && rb.transform.position.x - player.transform.position.x <= 0)
        {
            
        }
    }
}
