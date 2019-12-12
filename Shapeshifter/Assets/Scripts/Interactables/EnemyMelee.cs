using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
{
    public GameObject player;

    public GameObject healthBar;

    public float damage = 5f;
    public int followRange = 40;

    public bool delayFinished = false;
    bool delayMovementFinished;
    bool playerMoved;

    public float attackSpeedReload;
    private float attackSpeedReset;
    public float hpTimer = 2;
    public int maxSpeed;

    CharacterStats myStats;

    // Start is called before the first frame update
    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        attackSpeedReload = 2;

    }

    // Update is called once per frame
    public override void FixedUpdate()
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

            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (player.transform.position.x - rb.transform.position.x >= -followRange && player.transform.position.x - rb.transform.position.x <= 0 ||
            rb.transform.position.x - player.transform.position.x >= -followRange && rb.transform.position.x - player.transform.position.x <= 0)
        {
            if (delayFinished == false)
            {
                StartCoroutine(delay(1));
            }
            else
            {
                if (delayFinished)
                {
                    if (player.transform.position.x - rb.transform.position.x >= -followRange && player.transform.position.x - rb.transform.position.x <= 0)
                    {
                        Debug.Log("inRange");

                        RaycastHit2D raycastTarget = Physics2D.Raycast(new Vector2(transform.position.x - 1.1f, transform.position.y - 0.5f), -transform.right, .5f);
                        int multiplier = 1;

                        if (raycastTarget.transform != null && raycastTarget.transform.tag == "map")
                        {
                            multiplier = 2;
                        }

                        if (playerMoved == false)
                        {
                            StartCoroutine(delayMovement(1));
                            if (delayMovementFinished == true)
                            {
                                rb.AddForce(-speed * transform.right * multiplier);
                                
                            }
                        }
                        else
                        {
                            rb.AddForce(-speed * transform.right * multiplier);
                            
                        }
                    }
                    if (rb.transform.position.x - player.transform.position.x >= -followRange && rb.transform.position.x - player.transform.position.x <= 0)
                    {
                        RaycastHit2D raycastTarget = Physics2D.Raycast(new Vector2(transform.position.x + 1.1f, transform.position.y - 0.5f), transform.right, .5f);
                        int multiplier = 1;

                        if (raycastTarget.transform != null && raycastTarget.transform.tag == "map")
                        {
                            multiplier = 2;
                        }

                        if (playerMoved == false)
                        {
                            StartCoroutine(delayMovement(1));
                            if (delayMovementFinished == true)
                            {
                                rb.AddForce(speed * transform.right * multiplier);
                            }
                        }
                        else
                        {
                            rb.AddForce(speed * transform.right * multiplier);
                        }
                    }
                }
            }
        }
        if (attackSpeedReset > 0)
        {
            attackSpeedReset -= Time.deltaTime;
        }

        if (rb.velocity.x < -maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
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
    public IEnumerator delay(float time)
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
