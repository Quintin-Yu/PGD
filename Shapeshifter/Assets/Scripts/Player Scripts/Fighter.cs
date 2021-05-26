using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class is the fighter of the player
 * This class inherits from Class (A class as in fighter, archer, mage, cleric)
 * 
 * @Author Nicolaas Schuddeboom
 */

[System.Serializable]
public class Fighter : Class
{
    // The attack hit box. A variable is needed to store memory (Explained below.)
    [SerializeField] Collider2D attack;

    // CharacterStats to access and store more variables of the enemy and player (made by Quintin.) Other variables are self explanatory by name
    [Header("Variables")]
    CharacterStats myStats, playerStats;
    public Player playerScript;
    public GameObject enemyHPBar;
    Rigidbody2D rb;
    bool delayfinished = false;

    // Variables are self explanatory by name
    [Header("Combat")]
    public GameObject shield;
    public int knockback = 100;

    public Animator animator;
    public bool isCharging;
    public int chargeVelocity;
    private float chargeTimer = 0;

    private float chargeCooldown = 0;
    public Text textCooldown;

    // Initialize variables
    void Start()
    {
        shield.SetActive(false);

        playerStats = GetComponent<CharacterStats>();
        myStats = GetComponent<CharacterStats>();

        isCharging = false;
        playerScript = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Input for shield
    private void Update()
    {
        // If shield is active...
        if(Input.GetMouseButtonDown(1))
        {
            // Change speed, hud and activate the object
            playerScript.horizontalSpeedMultiplier = 0.5f;
            playerScript.hud.knightCooldowns[1].reloadImage.fillAmount = 1;
            shield.SetActive(true);
        }
        // If shield is not active because of releasing the button or because the fighter is charging...
        if (Input.GetMouseButtonUp(1) || isCharging)
        {
            // Change speed, hud and deactivate the object
            playerScript.horizontalSpeedMultiplier = 1;
            playerScript.hud.knightCooldowns[1].reloadImage.fillAmount = 0;
            shield.SetActive(false);
        }
    }

    // Charge
    private void FixedUpdate()
    {
        // If the fighter is charging...
        if (isCharging)
        {
            animator.SetBool("IsCharging", true);

            // Reduce time charging, change hud, add a force and lock the player's movement
            chargeTimer -= Time.deltaTime;
            player.hud.knightCooldowns[2].StartCooldown(chargeCooldown);
            rb.AddForce(transform.right * chargeVelocity * Time.deltaTime * 1000);
            playerScript.lockMovement = true;

            // If the timer reaches zero...
            if (chargeTimer < 0)
            {
                // Stop charging and reset variables (reset speed and unlock movement)
                isCharging = false;
                animator.SetBool("IsCharging", false);
                playerScript.maxMovementSpeed /= 3;
                playerScript.lockMovement = false;
            }
        }
        else
        {
            // Else reduce cooldown of charge
            if (chargeCooldown >= 0)
            {
                chargeCooldown -= Time.deltaTime;
            }
        }
    }

    // Charge collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player isn't charging, nothing needs to be done. But if it is charging...
        if (isCharging)
        {
            if (collision.gameObject.tag == "Breakable")
            {
                GetComponent<BoxCollider2D>().enabled = false;

                Destroy(collision.gameObject);
            }

            // Check if the collision is an enemy-
            if (collision.gameObject.tag == "EnemyRanged" || collision.gameObject.tag == "EnemyMelee")
            {
                // Check if it is melee and lock it's movement (an archer is stationary and doesn't need his movement locked)
                if (collision.gameObject.tag == "EnemyMelee")
                {
                    collision.gameObject.GetComponent<EnemyMelee>().knockbackTimer = 2;
                }

                // Check direction the fighter is charging and add a knockback in the right direction
                if (chargeVelocity < 0)
                {
                    collision.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-75, 10);
                }
                else
                {
                    collision.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(75, 10);
                }

                // Create/Change a few variables
                CombatController enemyCombat = collision.transform.GetComponent<CombatController>();
                myStats = collision.transform.GetComponent<CharacterStats>();

                // Deal damage to the enemy
                this.GetComponent<CombatController>().Attack(myStats);
                
                if (collision.gameObject.GetComponent<EnemyMelee>() != null)
                {
                    // Show healthbar and set knockback timer (repeat?)
                    EnemyMelee enemyScript = collision.gameObject.GetComponent<EnemyMelee>();

                    enemyScript.healthBar.SetActive(true);
                    enemyScript.hpTimer = 2;

                    enemyScript.knockbackTimer = 1;
                }
                if (collision.gameObject.GetComponent<EnemyRanged>() != null)
                {
                    // Show healthbar
                    collision.transform.GetComponent<EnemyRanged>().healthBar.SetActive(true);
                    collision.transform.GetComponent<EnemyRanged>().hpTimer = 2;
                }
                // To avoid too much duplicate damage and odd collision, push the enemy faster into the air
                collision.transform.Translate(new Vector2(0, 0.25f));
            }
        }
    }

    // Attack
    public override void Attack()
    {
        // Check if the fighter is able to attack
        if (!delayfinished)
        {
            // Lock the ability to attack for a few moments
            delayfinished = true;
            StartCoroutine(MeleeAttack(0.3f));

            // Start a few coroutines and change a variable in the player
            playerScript.isAttacking = true;
            StartCoroutine(playerScript.LockMovement(0.5f));
            StartCoroutine(playerScript.AttackDone(0.5f));

            // Play audio
            FindObjectOfType<AudioManager>().Play("Miss Melee");

            // Create a variable to check what we hit (a list of enemies in the attack hitbox)
            List<GameObject> gameObjects = attack.GetComponent<FighterAttack>().objectsInHitbox;

            // Check all objects in this list
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                // If it is an enemy
                if (gameObjects[i].tag == "EnemyRanged" || gameObjects[i].tag == "EnemyMelee")
                {
                    // Create/Change a few variables
                    CombatController enemyCombat = gameObjects[i].transform.GetComponent<CombatController>();
                    myStats = gameObjects[i].transform.GetComponent<CharacterStats>();

                    if (enemyCombat != null)
                    {
                        // Print a comment
                        Debug.Log(enemyCombat + " " + myStats);

                        // Deal damage
                        this.GetComponent<CombatController>().Attack(myStats);

                        // Because of weird behaviour if an object is destroyed in a list and everything being erased, use try/catch
                        try
                        {
                            // If the enemy is still alive...
                            if (gameObjects[i] != null)
                            {
                                // Show health
                                if (gameObjects[i].GetComponent<EnemyMelee>() != null)
                                {
                                    // And set knockback timer for melee enemy (an archer is stationary and doesn't need his movement locked)
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

                                // Reset x velocity and then add a knockback in the right direction
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
                    // Play audio
                    FindObjectOfType<AudioManager>().Play("Hit Melee");

                    return;
                }
            }
        }
    }

    // Charge inheritance
    public override void Ability()
    {
        // If the knight isn't charging and we can charge...
        if (!isCharging && chargeCooldown < 0)
        {
            // Set cooldown, set a bool so the fighter knows it is charging, change max speed and set timer for how long we charge
            chargeCooldown = 5;
            isCharging = true;
            playerScript.maxMovementSpeed *= 3;
            chargeTimer = 1.5f;

            // Change the vector so we're charging the right direction (if it is wrong in the first place)
            if (playerScript.flipped == (chargeVelocity > 0))
            {
                chargeVelocity *= -1;
            }
        }
    }

    // Enumerator for unlocking movement
    public IEnumerator MeleeAttack(float time)
    {
        yield return new WaitForSeconds(time);
        delayfinished = false;
    }
}
