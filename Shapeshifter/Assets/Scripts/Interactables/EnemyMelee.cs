using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public GroundCollider groundCollider;
    public GameObject player;

    public GameObject healthBar;

    public float damage = 5f;

    bool delayFinished = false;
    bool delayMovementFinished;
    bool playerMoved;
    float speed = 0.1f;
    int jumpHeight;
    int hp = 1;

    public float attackSpeedReload;
    private float attackSpeedReset;
    public float hpTimer = 2;

    CharacterStats myStats;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        attackSpeedReload = 2;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        hpTimer -= Time.deltaTime;
        if (hpTimer <= 0)
        {
            healthBar.SetActive(false);
        }

        if (player.transform.position.x - rb.transform.position.x >= -1 && player.transform.position.x - rb.transform.position.x <= 0 ||
            rb.transform.position.x - player.transform.position.x >= -1 && rb.transform.position.x - player.transform.position.x <= 0)
        {
            playerMoved = false;
            delayMovementFinished = false;
        }

        if (player.transform.position.x - rb.transform.position.x >= -40 && player.transform.position.x - rb.transform.position.x <= 0 ||
            rb.transform.position.x - player.transform.position.x >= -40 && rb.transform.position.x - player.transform.position.x <= 0)
        {
            if (delayFinished == false)
            {
                StartCoroutine(delay(1));
            }
            else
            {
                if (delayFinished)
                {
                    if (player.transform.position.x - rb.transform.position.x >= -40 && player.transform.position.x - rb.transform.position.x <= 0)
                    {
                        if (playerMoved == false)
                        {
                            StartCoroutine(delayMovement(1));
                            if (delayMovementFinished == true)
                            {
                                transform.Translate(-speed, 0f, 0f);
                            }
                        }
                        else
                        {
                            transform.Translate(-speed, 0f, 0f);
                        }
                    }
                    if (rb.transform.position.x - player.transform.position.x >= -40 && rb.transform.position.x - player.transform.position.x <= 0)
                    {
                        if (playerMoved == false)
                        {
                            StartCoroutine(delayMovement(1));
                            if (delayMovementFinished == true)
                            {
                                transform.Translate(speed, 0f, 0f);
                            }
                        }
                        else
                        {
                            transform.Translate(speed, 0f, 0f);
                        }
                    }
                }
            }
        }
            if (attackSpeedReset > 0)
            {
                attackSpeedReset -= Time.deltaTime;
            }
    }
    public void OnCollisionStay2D(Collision2D collision)
    { 
        if(attackSpeedReset <= 0) {
            if (collision.gameObject.tag.Equals("Player"))
            {
                CombatController playerCombat = collision.gameObject.GetComponent<CombatController>();
                myStats = collision.transform.GetComponent<CharacterStats>();

                if (playerCombat != null)
                {
                    this.GetComponent<CombatController>().Attack(myStats);
                    attackSpeedReset = attackSpeedReload;
                }
            }
        }
    }
    IEnumerator delay(float time)
    {
        yield return new WaitForSeconds(time);
        delayFinished = true;
    }
    IEnumerator delayMovement(float time)
    {
        yield return new WaitForSeconds(time);
        playerMoved = true;
        delayMovementFinished = true;
    }
    
}
