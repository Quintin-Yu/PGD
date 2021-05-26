using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class is a child of the GameCharacter script
 * 
 * @Author Nicolaas Schuddeboom
 */

public class Player : GameCharacter
{
    // Variables
    [Header("General")]
    public HUD hud;                                     // Hud
    public Text mageCooldown;                           // Cooldown between firing for the mage display
    public Animator animator;

    [HideInInspector] public bool lockMovement = false; // A variable for locking the player's movement. This variable is hidden in the inspector

    [Header("Movement")]
    public int maxMovementSpeed;                        // Max movement speed of the player
    public int minMovementSpeed;                        // Min movement speed of the player
    public float movementFriction;                      // Friction of movement

    [HideInInspector] public float horizontalSpeedMultiplier;

    // Basic variables
    //bool grounded;

    float inputSpeed;
    bool jump;

    [Header("Combat related variables")]
    public bool flipped;
    public bool canFlip;

    public bool isAttacking;
    public bool isDefending;

    //Variables for the attack reloads
    public float archerAttackCooldown;
    public float warriorAttackCooldown;

    [Header("Change class")]
    //Variables for the class switching cooldown
    public float transformCooldown;
    public bool isAllowedToChange;

    public int classIndex;

    public bool knockbackBool = false;
    public bool canTransform = true;                    // Boolean for knowing if the player can switch between classes
    public Class[] classes;                             // List of the player's classes

    public ParticleSystem smokeEffectShapeShift;
    private Vector3 offsetSmoke;

    [HideInInspector] public float knockBackStartTimer = 0;
    //[SerializeField] Animator animator;

    public Mage mage;

    private void Start()
    {
        horizontalSpeedMultiplier = 1;
        //grounded = true;
        //flipped = false;
        maxMovementSpeed = 1;
        minMovementSpeed = 0;

        // Get rigidbody
        rb = GetComponent<Rigidbody2D>();

        // Set cooldowns
        archerAttackCooldown = 2f;
        warriorAttackCooldown = 1f;
        transformCooldown = 3;

        offsetSmoke = new Vector3(0, 1.5f, 0);

        // Change to right class
        ShiftClass();
    }

    private void Update()
    {
        // If the knockback isn't active, run the attack manager
        if (!knockbackBool)
        {
            AttackManager();
        }

        // If the player can change, run the change manager
        if (isAllowedToChange)
        {
            ClassChangeManager();
        }

        // Get input for jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // If the player is grounded...
            if (groundCollider.grounded)
            {
                // Set the jump inputs to true
                jump = true;
                animator.SetBool("IsJumping", true);
            }
        }

        // Get input for movement
        inputSpeed = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime * horizontalSpeedMultiplier;

        // If the player is defending, half its movement speed
        if (isDefending)
        {
            inputSpeed /= 2;
        }

        // Flip the player in the right direction
        if (inputSpeed != 0)
        {
            if ((inputSpeed < 0) != flipped)
            {
                flipped = !flipped;

                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
            }
        }
    }

    private void FixedUpdate()
    {
        // Reduce knockback timer (if necessairy)
        if (knockBackStartTimer > 0)
        {
            knockBackStartTimer -= Time.deltaTime;
        }

        // If the player is grounded and the player was knockedback
        if (groundCollider.IsGrounded && knockBackStartTimer <= 0)
        {
            // Give the player control again
            knockbackBool = false;
            animator.SetBool("isgrounded", true);
        }
        else
        {
            animator.SetBool("isgrounded", false);
        }

        //Changes the animator
        animator.SetFloat("Speed", Mathf.Abs(inputSpeed));
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetInteger("Class", classIndex);

        // If the player isn't grounded, give it a jumping animation
        if (!groundCollider.grounded)
        {
            animator.SetBool("IsJumping", false);
        }

        // Add friction
        if (!knockbackBool)
        {
            // Left/Right friction
            rb.velocity = new Vector2(rb.velocity.x * movementFriction, rb.velocity.y);

            // If we go too fast, slow down
            if (rb.velocity.x < -maxMovementSpeed)
            {
                rb.velocity = new Vector2(-maxMovementSpeed, rb.velocity.y);
            }
            if (rb.velocity.x > maxMovementSpeed)
            {
                rb.velocity = new Vector2(maxMovementSpeed, rb.velocity.y);
            }
        }

        // If the player isn't allowed to move, stop and do nothing of the following.
        if (lockMovement || knockbackBool)
        {
            return;
        }

        // Add a force with the input of the player.
        rb.AddForce(inputSpeed * transform.right);

        // And if we go too slow, stop completely
        if (rb.velocity.x > -minMovementSpeed && rb.velocity.x < minMovementSpeed)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // Jump
        if (jump)
        {
            jump = false;

            classes[classIndex].Jump(this);
        }
    }

    // Change class
    void ShiftClass()
    {
        smokeEffectShapeShift.transform.position = this.gameObject.transform.position - offsetSmoke;
        smokeEffectShapeShift.Play();

        // Set variables to match the class
        speed = classes[classIndex].speed;
        jumpHeight = classes[classIndex].jumpHeight;

        // Give hud animation
        hud.playAnimation(classIndex);
        FindObjectOfType<AudioManager>().Play("Transform");

        // The player isn't allowed to change and run a timer for when he can change
        isAllowedToChange = false;
        StartCoroutine(ShapeShiftCooldown(transformCooldown));
    }

    // Class manager
    private void ClassChangeManager()
    {
        // If the fighter input is given, change to fighter if not already a fighter
        if (Input.GetKeyDown(KeyCode.Alpha1) && classIndex != 0)
        {
            classIndex = 0;
            ShiftClass();
        }

        // If the archer input is given, change to archer if not already an archer
        if (Input.GetKeyDown(KeyCode.Alpha2) && classIndex != 1)
        {
            classIndex = 1;
            ShiftClass();
        }

        // If the mage input is given, change to mage if not already a mage
        if (Input.GetKeyDown(KeyCode.Alpha3) && classIndex != 2)
        {
            classIndex = 2;
            ShiftClass();
        }
    }

    //Manages all the class attacks
    private void AttackManager()
    {
        switch (classIndex)
        {
            //Case for the warrior
            case 0:
                // If the e input is given, activate the fighter's ability
                if (Input.GetKeyDown("e"))
                {
                    classes[classIndex].Ability();
                    break;
                }

                // Melee attack
                if (Input.GetMouseButtonDown(0) && !recentlyAttacked)
                {
                    // Hud cooldown
                    hud.knightCooldowns[0].StartCooldown(warriorAttackCooldown);

                    // Run code for attacking damage
                    classes[classIndex].Attack();

                    // Start cooldown for attacking
                    recentlyAttacked = true;
                    StartCoroutine(AttackCooldown(warriorAttackCooldown));
                }
                break;

            //Case for the archer
            case 1:
                // If the'player wants to shoot...
                if (Input.GetMouseButtonDown(0) && !recentlyAttacked)
                {
                    // Run code for shooting arrow
                    classes[classIndex].Attack();

                    // Start attacking cooldown
                    recentlyAttacked = true;
                    StartCoroutine(AttackCooldown(archerAttackCooldown));

                    // Hud cooldown
                    hud.archerCooldowns[0].StartCooldown(archerAttackCooldown);
                }
                break;

            //Case for the mage
            case 2:
                // If the player wants to shoot a fireball, shoot it
                if (Input.GetMouseButtonDown(0))
                {
                    classes[classIndex].Attack();
                }
                break;
        }
    }
    
    // Lock movement
    public IEnumerator LockMovement(float time)
    {
        // Lock
        rb.velocity = Vector2.zero;
        lockMovement = true;

        // Wait
        yield return new WaitForSeconds(time);

        // Unlock
        lockMovement = false;
    }

    // Attacking
    public IEnumerator AttackDone(float time)
    {
        // Wait
        yield return new WaitForSeconds(time);

        // Not attacking
        isAttacking = false;
    }

    // Shift cooldown
    public IEnumerator ShapeShiftCooldown(float time)
    {
        // Cooldown
        yield return new WaitForSeconds(time);

        // Allowed to change
        isAllowedToChange = true;
    }

    private void OnDrawGizmos()
    {
        
    }
}
