using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
{
    public float damage;
    public int followRange;

    public bool delayFinished = false;
    bool delayMovementFinished;
    bool playerMoved;

    public float attackSpeedReload;
    private float attackSpeedReset;
    public float knockbackTimer;
    public int maxSpeed;

    CharacterStats myStats;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        damage = 5f;
        followRange = 40;
        knockbackTimer = 0;
        attackSpeedReload = 2;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        EnemyMeleeMovement();
        knockbackTimer -= Time.deltaTime;
        if (knockbackTimer <= 0)
        {
            delayFinished = true;
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

    private void EnemyMeleeMovement()
    {

        if (targetPlayer.transform.position.x - rb.transform.position.x >= -1 && targetPlayer.transform.position.x - rb.transform.position.x <= 0 ||
            rb.transform.position.x - targetPlayer.transform.position.x >= -1 && rb.transform.position.x - targetPlayer.transform.position.x <= 0)
        {
            playerMoved = false;
            delayMovementFinished = false;

            if (knockbackTimer < 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        if (targetPlayer.transform.position.x - rb.transform.position.x >= -followRange && targetPlayer.transform.position.x - rb.transform.position.x <= 0 ||
        rb.transform.position.x - targetPlayer.transform.position.x >= -followRange && rb.transform.position.x - targetPlayer.transform.position.x <= 0)
        {
            if (delayFinished == false || knockbackTimer > 0)
            {
                if (delayFinished == false)
                {
                    StartCoroutine(delay(1));
                }
            }
            else
            {
                if (delayFinished)
                {
                    if (targetPlayer.transform.position.x - rb.transform.position.x >= -followRange && targetPlayer.transform.position.x - rb.transform.position.x <= 0)
                    {
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
                    if (rb.transform.position.x - targetPlayer.transform.position.x >= -followRange && rb.transform.position.x - targetPlayer.transform.position.x <= 0)
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
    }

    public void OnCollisionStay2D(Collision2D collision)
    { 
        if(attackSpeedReset <= 0) {
            if (collision.gameObject.tag.Equals("Player") && !collision.gameObject.GetComponent<Fighter>().isCharging)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                collision.gameObject.GetComponent<Player>().knockbackBool = true;

                if (collision.transform.position.x < transform.position.x)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-400, 600));
                }
                else
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(400, 600));
                }
                

                CombatController playerCombat = collision.gameObject.GetComponent<CombatController>();
                myStats = collision.transform.GetComponent<CharacterStats>();

                if (playerCombat != null)
                {
                    if (collision.gameObject.GetComponent<Player>().isDefending)
                    {
                        this.GetComponent<CombatController>().mystats.strength.AddModifier(7);
                        
                        this.GetComponent<CombatController>().Attack(myStats);

                        this.GetComponent<CombatController>().mystats.strength.RemoveModifier(7);
                    }
                    else
                    {
                        this.GetComponent<CombatController>().Attack(myStats);
                    }
                    
                    attackSpeedReset = attackSpeedReload;
                }

                if (knockbackTimer < 1)
                {
                    knockbackTimer = 1;
                }
                rb.velocity = new Vector2(0, rb.velocity.y);
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
