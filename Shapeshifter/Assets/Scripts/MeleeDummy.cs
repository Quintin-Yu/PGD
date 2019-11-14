using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public GroundCollider groundCollider;
    public GameObject player;

    public float damage = 5f;

    float speed = 0.1f;
    int jumpHeight;
    int hp = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

    }
}
