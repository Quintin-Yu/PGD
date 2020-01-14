using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class is a child of the GameCharacter script
 * 
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

        // Change to right class
        ShiftClass();
    }

    private void Update()
    {
        if (!knockbackBool)
        {
            AttackManager();
        }

        if (isAllowedToChange)
        {
            ClassChangeManager();
        }

        // Get input for jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (groundCollider.grounded)
            {
                
                jump = true;
                animator.SetBool("IsJumping", true);
            }
        }

        // Get input for movement
        inputSpeed = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime * horizontalSpeedMultiplier;

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
        if (knockBackStartTimer > 0)
        {
            knockBackStartTimer -= Time.deltaTime;
        }

        if (groundCollider.IsGrounded && knockBackStartTimer <= 0)
        {
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

        if (!groundCollider.grounded)
        {
            animator.SetBool("IsJumping", false);
        }

        // And add friction
        if (!knockbackBool)
        {
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

        // If the player isn't allowed to move, do nothing.
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
        speed = classes[classIndex].speed;
        jumpHeight = classes[classIndex].jumpHeight;
        hud.playAnimation(classIndex);

        isAllowedToChange = false;
        StartCoroutine(ShapeShiftCooldown(transformCooldown));
    }

    private void ClassChangeManager()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && classIndex != 0)
        {
            classIndex = 0;
            ShiftClass();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && classIndex != 1)
        {
            classIndex = 1;
            ShiftClass();
        }

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
                if (Input.GetKeyDown("e"))
                {
                    classes[classIndex].Ability();
                    break;
                }

                if (Input.GetMouseButtonDown(0) && !recentlyAttacked)
                {
                    hud.knightCooldowns[0].StartCooldown(warriorAttackCooldown);
                    classes[classIndex].Attack();
                    recentlyAttacked = true;
                    StartCoroutine(AttackCooldown(warriorAttackCooldown));
                }
                break;

            //Case for the archer
            case 1:
                if (Input.GetMouseButtonDown(0) && !recentlyAttacked)
                {
                    classes[classIndex].Attack();
                    recentlyAttacked = true;
                    StartCoroutine(AttackCooldown(archerAttackCooldown));
                    hud.archerCooldowns[0].StartCooldown(archerAttackCooldown);
                }
                break;

            //Case for the mage
            case 2:
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
        rb.velocity = Vector2.zero;
        lockMovement = true;

        yield return new WaitForSeconds(time);

        lockMovement = false;
    }

    public IEnumerator AttackDone(float time)
    {
        yield return new WaitForSeconds(time);

        isAttacking = false;
    }

    public IEnumerator ShapeShiftCooldown(float time)
    {
        yield return new WaitForSeconds(time);

        isAllowedToChange = true;
    }

    private void OnDrawGizmos()
    {
        
    }
}
