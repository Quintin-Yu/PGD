using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Class
{
    public int speed;
    public int jumpHeight;
    

    public Class(int speed, int jumpHeight)
    {
        this.speed = speed * 1000;
        this.jumpHeight = jumpHeight;
    }

    public virtual void Attack(GameObject attack, GameObject origin)
    {

    }

    public virtual void Jump(Player player)
    {
        if (player.groundCollider.IsGrounded)
        {
            player.rb.AddForce(jumpHeight * player.transform.up);
        }
    }
}

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public GroundCollider groundCollider;
    public HUD hud;

    public GameObject equipmentScreen;
    private bool equipmentScreenActive = false;

    [HideInInspector] public bool lockMovement = false;

    List<Class> classes = new List<Class>();
    [SerializeField] List<GameObject> classesAttacks = new List<GameObject>();

    [SerializeField] Transform sprite;

    [SerializeField] int maxMovementSpeed;
    [SerializeField] float movementFriction;

    int speed;
    int jumpHeight;
    bool grounded = true;

    float inputSpeed;
    bool jump;

    bool flipped = false;

    public int classIndex = 0;

    [SerializeField] Animator animator;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        classes.Add(new Fighter());
        classes.Add(new Archer());
        classes.Add(new Mage());

        ShiftClass();
    }

    private void Update()
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

        if (Input.GetMouseButtonDown(0))
        {
            classes[classIndex].Attack(classesAttacks[classIndex], this.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        inputSpeed = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;

        if (inputSpeed != 0)
        {
            if ((inputSpeed < 0) != flipped)
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
        if (lockMovement)
        {
            return;
        }

        rb.AddForce(inputSpeed * transform.right);

        rb.velocity = new Vector2(rb.velocity.x * movementFriction, rb.velocity.y);

        if (rb.velocity.x < -maxMovementSpeed)
        {
            rb.velocity = new Vector2(-maxMovementSpeed, rb.velocity.y);
        }
        if (rb.velocity.x >  maxMovementSpeed)
        {
            rb.velocity = new Vector2( maxMovementSpeed, rb.velocity.y);
        }
        if (jump)
        {
            jump = false;

            classes[classIndex].Jump(this);
        }
    }

    void ShiftClass()
    {
        speed = classes[classIndex].speed;
        jumpHeight = classes[classIndex].jumpHeight;
        hud.playAnimation(classIndex);
    }

    public IEnumerator LockMovement(float time)
    {
        rb.velocity = Vector2.zero;
        lockMovement = true;

        yield return new WaitForSeconds(time);

        lockMovement = false;
    }
}
