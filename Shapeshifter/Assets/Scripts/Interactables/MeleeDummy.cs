using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public GroundCollider groundCollider;
    public GameObject player;

    public float damage;

    public float speed;
    public int jumpHeight;
    public int hp;
    
    // Start is called before the first frame update
    void Start()
    {
        damage = 5;
        speed = 0.1f;
        hp = 1;

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

    }
}
