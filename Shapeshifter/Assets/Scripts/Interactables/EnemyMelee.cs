using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is a child of the Enemy class.
 * It contains the movement, attack and how the enemy melee can get damaged
 */

public class EnemyMelee : Enemy
{
    [Header("General")]
    public float damage;
    public int followRange;

    public bool delayFinished = false;
    bool delayMovementFinished;
    bool playerMoved;

    [Header("Timers")]
    public float attackSpeedReload;
    private float attackSpeedReset;
    public float knockbackTimer;

    [Header("Needed variables")]
    public int maxSpeed;
    public float Float;
    public Animator animator;
    bool isFlipped = false;

    CharacterStats myStats;
    FloorCheck fc;
   

    public CircleCollider2D ignoreGroundCollider;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        damage = 5f;
        //followRange = 40;
        knockbackTimer = 0;
        attackSpeedReload = 2;

        fc = GetComponentInChildren<FloorCheck>();
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
            //give the ai a way of response time, so it doesnt move at the same time as the player does
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
                    //moving the enemymelee from the left side of the player and activating animation
                    if (targetPlayer.transform.position.x - rb.transform.position.x >= -followRange && targetPlayer.transform.position.x - rb.transform.position.x <= 0)
                    {
                        animator.SetBool("isMoving", true);

                        //flipping the enemy melee hp bar and sprite
                        if (isFlipped == true)
                        {
                            healthBar.gameObject.transform.Rotate(0, 180, 0);
                            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                            isFlipped = false;
                        }

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
                                if (fc.isHitting)
                                {
                                    animator.SetBool("isMoving", true);
                                    rb.AddForce(-speed * transform.right * multiplier);
                                }
                                else
                                {
                                    if (groundCollider.IsGrounded)
                                    {
                                        animator.SetBool("isMoving", false);
                                        rb.velocity = Vector3.zero;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (fc.isHitting) {
                                animator.SetBool("isMoving", true);
                                rb.AddForce(-speed * transform.right * multiplier);
                            }
                            else
                            {
                                if (groundCollider.IsGrounded)
                                {
                                    animator.SetBool("isMoving", false);
                                    rb.velocity = Vector3.zero;
                                }
                            }
                        }
                    }
                    //movement/animation again, but this time from the right side of the player
                    else if (rb.transform.position.x - targetPlayer.transform.position.x >= -followRange && rb.transform.position.x - targetPlayer.transform.position.x <= 0)
                    {
                        animator.SetBool("isMoving", true);
                        if (isFlipped == false)
                        {
                            healthBar.gameObject.transform.Rotate(0, 180, 0);
                            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                            isFlipped = true;
                        }
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
                                if (fc.isHitting)
                                {
                                    animator.SetBool("isMoving", true);
                                    rb.AddForce(speed * transform.right * multiplier);
                                }
                                else
                                {
                                    if (groundCollider.IsGrounded)
                                    {
                                        animator.SetBool("isMoving", false);
                                        rb.velocity = Vector3.zero;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (fc.isHitting)
                            {
                                animator.SetBool("isMoving", true);
                                rb.AddForce(speed * transform.right * multiplier);
                            }
                            else
                            {
                                if (groundCollider.IsGrounded)
                                {
                                    animator.SetBool("isMoving", false);
                                    rb.velocity = Vector3.zero;
                                }
                            }

                        }
                    }
                    else
                    {
                        animator.SetBool("isMoving", false);
                    }
                }
            }
        }
    }

    //Enemy melee units will ignore enemy ranged units collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("EnemyRanged"))
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<BoxCollider2D>());
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<CircleCollider2D>());
            Physics2D.IgnoreCollision(ignoreGroundCollider, collision.gameObject.GetComponent<BoxCollider2D>());
            Physics2D.IgnoreCollision(ignoreGroundCollider, collision.gameObject.GetComponent<CircleCollider2D>());
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //The attack of the enemy melee
        if(attackSpeedReset <= 0) {
            if (collision.gameObject.tag.Equals("Player") && !collision.gameObject.GetComponent<Fighter>().isCharging)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                collision.gameObject.GetComponent<Player>().knockbackBool = true;
                collision.gameObject.GetComponent<Player>().knockBackStartTimer = 0.2f;

                //add knockback after hitting the player
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

                //applying damage and animation
                if (playerCombat != null)
                {
                    this.GetComponent<CombatController>().Attack(myStats);

                    if (collision.gameObject.GetComponent<Player>().isDefending)
                    {
                        animator.SetBool("isAttacking", true);
                        this.GetComponent<CombatController>().mystats.strength.AddModifier(7);

                        this.GetComponent<CombatController>().Attack(myStats);

                        this.GetComponent<CombatController>().mystats.strength.RemoveModifier(7);
                        StartCoroutine(delayAnimation(1));
                    }
                    else
                    {
                        animator.SetBool("isAttacking", true);
                        this.GetComponent<CombatController>().Attack(myStats);
                        StartCoroutine(delayAnimation(1));
                    }
                    FindObjectOfType<AudioManager>().Play("Miss Melee"); 
                    FindObjectOfType<AudioManager>().Play("Hit Melee");
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

    public IEnumerator delayAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("isAttacking", false);
    }

}
