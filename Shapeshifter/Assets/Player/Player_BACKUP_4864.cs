using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
[RequireComponent(typeof(CharacterStats))]
public class Class : CharacterStats
{
    public int speed;
    public int jumpHeight;

    public virtual void Attack()
    {

    }

    public virtual void Jump(Player player)
    {
        if (player.groundCollider.IsGrounded)
        {
            player.rb.AddForce(jumpHeight * player.transform.up);
        }
    }

    public override void TakeDamage(float strength)
    {
       base.TakeDamage(strength);

        healthBar.fillAmount = CurrentHealth / maxHealth;
    }

    public override void Die()
    {
        base.Die();

        SceneManager.LoadScene("GameOver");
    }
}

public class Player : MonoBehaviour
{
    // Variables
    public Rigidbody2D rb;                  // Rigidbody for the physics
    public GroundCollider groundCollider;   // This collider checks if the player is standing on the ground
    public HUD hud;                         // Hud
    public Text mageCooldown;               // Cooldown between firing for the mage display
    public Animator animator;

    public GameObject equipmentScreen;          //
    private bool equipmentScreenActive = false; // 
    public bool canTransform = true;            // Boolean for knowing if the player can switch between classes

    [HideInInspector] public bool lockMovement = false; // A variable for locking the player's movement

    [SerializeField] Class[] classes;                                           // List of the player's classes

    [SerializeField] int maxMovementSpeed;      // Max movement speed of the player
    [SerializeField] int minMovementSpeed;      // Max movement speed of the player
    [SerializeField] float movementFriction;    // Friction of movement

    // Basic variables
    public int speed;
    int jumpHeight;
    bool grounded = true;

     float inputSpeed;
    bool jump;

    public bool flipped = false;

    public bool canFlip = true;
    public bool isAttacking;

    //Variables for the arrow reload
    private float arrowReload;
    public float arrowReset;

    private float readySwordReload;
    public float readySwordReset;

    //Variables for the class switching cooldown
    private float transformCooldown;
    public float transformCooldownReset;

    public int classIndex = 0;

    //[SerializeField] Animator animator;


    [SerializeField] Mage mage;

    private void Start()
    {
        // Get rigidbody
        rb = GetComponent<Rigidbody2D>();

        // Set cooldowns
        arrowReset = 2f;
        readySwordReset = 1f;

        // Set class shift cooldown
        transformCooldownReset = 3;

        // Change to fighter
        ShiftClass();
    }

    private void Update()
    {
        // If the player can shift between class, change class. Else reduce cooldown.
        if (transformCooldown <= 0 && canTransform)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                classIndex = 0;
                ShiftClass();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                classIndex = 1;
                ShiftClass();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                classIndex = 2;
                ShiftClass();
            }
        }
        else
        {
            transformCooldown -= Time.deltaTime;
        }

        //Attack warrior
        if (classIndex == 0 && readySwordReload <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                classes[classIndex].Attack();
                readySwordReload = readySwordReset;
            }
        }

        //If classIndex == 1 (aka the archer) he has to wait untill the reload time is completed
        if (classIndex == 1 && arrowReload <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                classes[classIndex].Attack();
                arrowReload = arrowReset;
            }
        }

        //Attack mage
        if (classIndex == 2 && groundCollider.IsGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                classes[classIndex].Attack();
            }
        }

        //When an arrow is shot this statement will activate. Player is only allowed to shoot once the timer is finished.
        if (arrowReload > 0)
        {
            arrowReload -= Time.deltaTime;
        } else if(readySwordReload > 0)
        {
            readySwordReload -= Time.deltaTime;
        }

        // Get input for jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        // Get input for movement
        inputSpeed = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;

        // Flip the player in the right direction
        if (inputSpeed != 0)
        {
            if ((inputSpeed < 0) != flipped && canFlip)
            {
                flipped = !flipped;

                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
            }
        }

        //opens equipment screen
        if (Input.GetKeyDown(KeyCode.E) && !equipmentScreenActive)
        {
            equipmentScreen.SetActive(true);
            equipmentScreenActive = true;
        }
        //closes equipment screen
        else if (Input.GetKeyDown(KeyCode.E) && equipmentScreenActive)
        {
            equipmentScreen.SetActive(false);
            equipmentScreenActive = false;
        }
    }

    private void FixedUpdate()
    {
        //Changes the animator
        animator.SetFloat("Speed", Mathf.Abs(inputSpeed));
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetInteger("Class", classIndex);

        if (!groundCollider.grounded)
        {
            animator.SetBool("IsJumping", false);
        }

        // Mage cooldown counter
        if (mage.nextFireTime - Time.time >= 0)
        {
            mageCooldown.enabled = true;
            mageCooldown.text = (Mathf.Round(mage.nextFireTime - Time.time)).ToString();
        }
        else
        {
            mageCooldown.enabled = false;
        }
        
        // If the player isn't allowed to move, do nothing.
        if (lockMovement)
        {
            return;
        }

        // Add a force with the input of the player.
        rb.AddForce(inputSpeed * transform.right);

        // And add friction
        rb.velocity = new Vector2(rb.velocity.x * movementFriction, rb.velocity.y);

        // If we go too fast, slow down
        if (rb.velocity.x < -maxMovementSpeed)
        {
            rb.velocity = new Vector2(-maxMovementSpeed, rb.velocity.y);
        }
        if (rb.velocity.x >  maxMovementSpeed)
        {
            rb.velocity = new Vector2( maxMovementSpeed, rb.velocity.y);
        }

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

        transformCooldown = transformCooldownReset;
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
}
