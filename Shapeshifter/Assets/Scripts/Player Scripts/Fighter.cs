using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Fighter : Class
{
    [SerializeField] Collider2D attack;
    [Header("Variables")]
    CharacterStats myStats, playerStats;
    public Player playerScript;
    public GameObject enemyHPBar;
    Rigidbody2D rb;
    bool delayfinished = false;

    [Header("Combat")]
    public GameObject shield;
    public int knockback = 100;

    public bool isCharging;
    public int chargeVelocity;
    private float chargeTimer = 0;

    private float chargeCooldown = 0;
    public Text textCooldown;

    void Start()
    {
        shield.SetActive(false);

        playerStats = GetComponent<CharacterStats>();
        myStats = GetComponent<CharacterStats>();

        isCharging = false;
        playerScript = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            playerScript.horizontalSpeedMultiplier = 0.5f;
            playerScript.hud.knightCooldowns[1].reloadImage.fillAmount = 1;
            shield.SetActive(true);
        }
        if (Input.GetMouseButtonUp(1) || isCharging)
        {
            playerScript.horizontalSpeedMultiplier = 1;
            playerScript.hud.knightCooldowns[1].reloadImage.fillAmount = 0;
            shield.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (isCharging)
        {
            chargeTimer -= Time.deltaTime;
            player.hud.knightCooldowns[2].StartCooldown(chargeCooldown);
            rb.AddForce(transform.right * chargeVelocity * Time.deltaTime * 1000);
            playerScript.lockMovement = true;

            if (chargeTimer < 0)
            {
                isCharging = false;
                playerScript.maxMovementSpeed /= 3;
                playerScript.lockMovement = false;
            }
        }
        else
        {
            if (chargeCooldown >= 0)
            {
                chargeCooldown -= Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharging)
        {
            if (collision.gameObject.tag == "EnemyRanged" || collision.gameObject.tag == "EnemyMelee")
            {
                if (collision.gameObject.tag == "EnemyMelee")
                {
                    collision.gameObject.GetComponent<EnemyMelee>().knockbackTimer = 2;
                }

                if (chargeVelocity < 0)
                {
                    collision.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-75, 10);
                }
                else
                {
                    collision.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(75, 10);
                }

                CombatController enemyCombat = collision.transform.GetComponent<CombatController>();
                myStats = collision.transform.GetComponent<CharacterStats>();
                this.GetComponent<CombatController>().Attack(myStats);

                if (collision.gameObject.GetComponent<EnemyMelee>() != null)
                {
                    EnemyMelee enemyScript = collision.gameObject.GetComponent<EnemyMelee>();

                    enemyScript.healthBar.SetActive(true);
                    enemyScript.hpTimer = 2;

                    enemyScript.knockbackTimer = 1;
                }
                if (collision.gameObject.GetComponent<EnemyRanged>() != null)
                {
                    collision.transform.GetComponent<EnemyRanged>().healthBar.SetActive(true);
                    collision.transform.GetComponent<EnemyRanged>().hpTimer = 2;
                }
                collision.transform.Translate(new Vector2(0, 0.25f));
            }
        }
    }

    public override void Attack()
    {
        if (!delayfinished)
        {
            delayfinished = true;
            StartCoroutine(MeleeAttack(0.3f));

            playerScript.isAttacking = true;
            StartCoroutine(playerScript.LockMovement(0.5f));
            StartCoroutine(playerScript.AttackDone(0.5f));

            FindObjectOfType<AudioManager>().Play("Miss Melee");

            List<GameObject> gameObjects = attack.GetComponent<FighterAttack>().objectsInHitbox;

            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (gameObjects[i].tag == "EnemyRanged" || gameObjects[i].tag == "EnemyMelee")
                {
                    //GameObject.Destroy(gameObjects[i].transform.parent.gameObject);
                    CombatController enemyCombat = gameObjects[i].transform.GetComponent<CombatController>();
                    myStats = gameObjects[i].transform.GetComponent<CharacterStats>();

                    if (enemyCombat != null)
                    {
                        Debug.Log(enemyCombat + " " + myStats);


                        this.GetComponent<CombatController>().Attack(myStats);

                        try
                        {
                            if (gameObjects[i] != null)
                            {
                                if (gameObjects[i].GetComponent<EnemyMelee>() != null)
                                {
                                    EnemyMelee enemyScript = gameObjects[i].GetComponent<EnemyMelee>();

                                    enemyScript.healthBar.SetActive(true);
                                    enemyScript.hpTimer = 2;

                                    enemyScript.knockbackTimer = 1;
                                }
                                if (gameObjects[i].GetComponent<EnemyRanged>() != null)
                                {
                                    gameObjects[i].transform.GetComponent<EnemyRanged>().healthBar.SetActive(true);
                                    gameObjects[i].transform.GetComponent<EnemyRanged>().hpTimer = 2;
                                }

                                gameObjects[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObjects[i].GetComponent<Rigidbody2D>().velocity.y);

                                if (playerScript.flipped)
                                {
                                    gameObjects[i].transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(-knockback, knockback));
                                }
                                else
                                {
                                    gameObjects[i].transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, knockback));
                                }
                            }
                        }
                        catch
                        {

                        }
                    }

                    FindObjectOfType<AudioManager>().Play("Hit Melee");
                    //GameObject.Destroy(gameObjects[i].transform.parent.gameObject);
                    //CombatController enemyCombat = gameObjects[i].transform.parent.GetComponent<CombatController>();
                    /*myStats = gameObjects[i].transform.parent.GetComponent<CharacterStats>();

                    if (enemyCombat != null)
                    {
                        Debug.Log(enemyCombat + " " + myStats);
                        this.GetComponent<CombatController>().Attack(myStats);
                    }*/

                    return;
                }
            }
        }
    }

    public override void Ability()
    {
        if (!isCharging && chargeCooldown < 0)
        {
            chargeCooldown = 5;

            isCharging = true;
            playerScript.maxMovementSpeed *= 3;
            chargeTimer = 1.5f;

            if (playerScript.flipped == (chargeVelocity > 0))
            {
                chargeVelocity *= -1;
            }
        }
    }

    public IEnumerator MeleeAttack(float time)
    {
        yield return new WaitForSeconds(time);
        delayfinished = false;
    }
}
