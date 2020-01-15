using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class is a child of the Class class.
 * It contains the mage abilities and attacks
 * @author Beau Wijkstra
 */

public class Mage : Class
{
    [Header("Cooldown")]
    //Cooldowns for the different abilities
    public float mageCooldown = 3f;
    public float nextFireTime;

    public float teleportCooldown = 10f;
    public float nextTeleportTime;
    bool canTeleport = true;

    [Header("Combat")]
    //Variables that change how the abilities interact
    private float arrowForce = 1000;
    private float charge = 0;
    public float teleportRange = 1f;

    Vector3 upSpeed = new Vector3(0, 0.3f, 0);
    Vector3 downSpeed = new Vector3(0, 0.3f, 0);

    private bool charging = false;
    private bool lClick = false;
    private bool attack = false;
    private bool inRange = true;
    private bool teleportKeyDown = false;
    private bool teleportKeyUp = false;
    private Vector3 fireballDistance = new Vector3(2, 0, 0);
    Vector3 mP;
    Vector3 outOfRangePosition;
    bool mouse = true;

    //Used in the gizmo's
    Vector3 playerGizmo = Vector3.zero;

    [Header("GameObjects")]
    //Gets other objects that are needed
    [SerializeField] GameObject magic;
    GameObject newArrow;
    public GameObject teleportSpot;
    private GameObject teleportP;
    [SerializeField] GameObject teleportParticle;
    public ParticleSystem particle;

    //Sets all the GameObject that need to be set, and sets some variables to their default state.
    private void Start()
    {
        teleportSpot = new GameObject();
        player = GetComponent<Player>();
        particle = GameObject.FindObjectOfType<ParticleSystem>();
        particle.Stop();
        teleportP = Instantiate(teleportParticle, Vector3.zero, Quaternion.identity);
        teleportP.SetActive(false);
    }

    private void Update()
    {
        //Gets the input for the teleport ability
        if (canTeleport && player.classIndex == 2)
        {
            if (Input.GetKey(KeyCode.E))
            {
                teleportKeyDown = true;
                teleportKeyUp = false;
            }

            else if (Input.GetKeyUp(KeyCode.E))
            {
                teleportKeyDown = false;
                teleportKeyUp = true;
            }
        }
    }

    private void FixedUpdate()
    {
        //If the key has been pressed, then the location of the teleport will be calculated
        if (teleportKeyDown)
        {
            GetTeleportLocation();
        }
        //If the key has been released the player will teleport to that spot and the cooldown of the teleport will begin
        else if (teleportKeyUp)
        {
            Teleport(teleportSpot.transform.position);
            nextTeleportTime = Time.time + teleportCooldown;
            teleportKeyUp = false;
        }

        //Sets the next teleport cooldown
        if (Time.time > nextTeleportTime)
        {
            canTeleport = true;
        }
        else canTeleport = false;

        
        playerGizmo = player.transform.position;
    }

    public override void Attack()
    {
        //Makes sure the main ability is off cooldown
        if (Time.time > nextFireTime)
        {
            player.isAttacking = true;
            player.canFlip = false;
            player.canTransform = false;

            //Gets the fireball and instantiates it so it can be used
            newArrow = GameObject.Instantiate(magic, this.transform.position, Quaternion.identity);
            newArrow.GetComponent<BoxCollider2D>().enabled = false;

            particle.Play();

            //Shoots the fireball in a certain direction
            Shoot(magic, this.gameObject, newArrow);

            //Sets the cooldown of the fireball
            nextFireTime = Time.time + mageCooldown;

            //Plays the cooldown animation in the HUD
            player.hud.mageCooldowns[0].StartCooldown(mageCooldown);

            //Makes sure the fireball is on the correct side of the player
            if (player.flipped)
            {
                particle.transform.position = new Vector3(newArrow.transform.position.x, newArrow.transform.position.y, -1);
                newArrow.transform.position = player.transform.position - fireballDistance;
            }
            else if (!player.flipped)
            {
                newArrow.transform.position = player.transform.position + fireballDistance;
                particle.transform.position = new Vector3(newArrow.transform.position.x, newArrow.transform.position.y, -1);
            }
        }
    }

    //Shoots the fireball in the direction of the mouse
    void Shoot(GameObject magic, GameObject origin, GameObject newArrow)
    {
        Vector3 direction;
        StartCoroutine(player.AttackDone(1));
        player.canTransform = true;
        player.canFlip = true;
        
        direction = Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position);
        direction.Normalize();

        newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);
        newArrow.GetComponent<BoxCollider2D>().enabled = true;
		FindObjectOfType<AudioManager>().Play("Fireball Cast");

        particle.Stop();
    }

    void GetTeleportLocation()
    {
        teleportP.SetActive(true);
        teleportSpot.SetActive(true);

        //Gets the mouse position and sets the z to 0
        mP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mP.z = 0;

        //Checks if the mouse position is outside the teleport range
        if (mP.x < player.transform.position.x - teleportRange || mP.x > player.transform.position.x + teleportRange
            || mP.y < player.transform.position.y - teleportRange || mP.y > player.transform.position.y + teleportRange)
        {
            inRange = false;
        }
        else inRange = true;

        //Makes 2 colliders to check if the current teleport position is making contact with an object
        //The collider will always be on the position of the mouse
        Collider2D collider = Physics2D.OverlapCircle(mP, 1);
        //The colliderOnSpot will always be at the position of the current teleportSpot
        Collider2D colliderOnSpot = Physics2D.OverlapCircle(teleportSpot.transform.position, 1);

        
        //Sets the position of the teleport if the mouse is not in range
        if (!inRange)
        {
            //Sets the max range of the teleport
            Vector3 diff = mP - player.transform.position;
            diff.z = 0;
            diff = Vector3.ClampMagnitude(diff, teleportRange);

            outOfRangePosition = player.transform.position + diff;

            //If the colliders don't collide with anything the mouse is the new teleportspot
            if (mouse)
            {
                teleportSpot.transform.position = outOfRangePosition;
            }

            //If the current teleportspot is making contact, it goes up
            //And if the mouse is making contact with something, the mouse won't be used as the new teleportspot
            if (colliderOnSpot)
            {
                teleportSpot.transform.position += upSpeed;

                if (collider)
                {
                    mouse = false;
                }
            }

            //If the current teleportspot is not making contact, and the mouse is not the same as the teleportspot, the teleportspot will go down
            //If the teleportspot is not making contact the new teleportspot will be calculated using a mix of the mouse's x and the teleportSpot's y
            if (colliderOnSpot != true)
            {
                if (colliderOnSpot != collider)
                {
                    teleportSpot.transform.position -= downSpeed;
                }
                teleportSpot.transform.position = new Vector3(outOfRangePosition.x, teleportSpot.transform.position.y, outOfRangePosition.z);
            }

            //Sets the mouse boolean to true since the mouse isn't colliding with anything
            if (collider != true)
            {
                mouse = true;
            }
        }

        //Sets the position of the teleport if it is in range
        if (inRange)
        {
            //If the mouse is not colliding with anything we can just use the mouse position
            if (mouse)
            {
                teleportSpot.transform.position = mP;
            }

            //If the current teleportspot is making contact, it goes up
            //And if the mouse is making contact with something, the mouse won't be used as the new teleportspot
            if (colliderOnSpot)
            {
                teleportSpot.transform.position += upSpeed;

                if (collider)
                {
                    mouse = false;
                }
            }

            //If the current teleportspot is not making contact, and the mouse is not the same as the teleportspot, the teleportspot will go down
            //If the teleportspot is not making contact the new teleportspot will be calculated using a mix of the mouse's x and the teleportSpot's y
            if (colliderOnSpot != true)
            {
                if (collider != colliderOnSpot)
                {
                    teleportSpot.transform.position -= downSpeed;
                }
                teleportSpot.transform.position = new Vector3(mP.x, teleportSpot.transform.position.y, mP.z);
            }

            //Sets the mouse boolean to true since the mouse isn't colliding with anything
            if (collider != true)
            {
                mouse = true;
            }
        }

        //Sets the teleportP (teleport particle) to the teleportSpot so the particles will be on the same position
        teleportP.transform.position = teleportSpot.transform.position;
    }

    void Teleport(Vector3 position)
    {
        //Sets the player position to the new position and sets the particles and the teleportspot to false so it won't interact with the world when not in use
        player.transform.position = position;
        teleportSpot.SetActive(false);
        teleportP.SetActive(false);
        player.hud.mageCooldowns[1].StartCooldown(teleportCooldown);
    }

    private void OnDrawGizmos()
    {
        //Draws a sphere on the mouse's position with a radius of 1
        Gizmos.DrawSphere(mP, 1);

        //Draws a line from the player to the teleport position when the mouse is out of range
        Gizmos.DrawLine(playerGizmo, outOfRangePosition);

        Gizmos.color = Color.blue;

        //Draws a sphere where the current teleportspot is
        if (teleportSpot != null)
        {
            Gizmos.DrawSphere(teleportSpot.transform.position, 1);
        }

        //Draws a wire sphere for the range of the teleport
        Gizmos.DrawWireSphere(playerGizmo, teleportRange);


    }
}
