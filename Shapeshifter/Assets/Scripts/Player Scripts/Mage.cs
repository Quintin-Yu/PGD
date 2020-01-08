using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mage : Class
{
    Vector3 direction;

    [Header("Cooldown")]
    //Decides how long the cooldown is between fires
    public float mageCooldown = 3f;
    public float nextFireTime;

    public float teleportCooldown = 10f;
    public float nextTeleportTime;
    bool canTeleport = true;

    //Decides how fast the fireball goes
    private float arrowForce = 1000;
    private float charge = 0;
    public float teleportRange = 1f;

    Vector3 upSpeed = new Vector3(0, 0.3f, 0);
    Vector3 downSpeed = new Vector3(0, 0.3f, 0);

    private bool charging = false;
    private bool lClick = false;
    private bool attack = false;
    private bool inRange = true;
    private Vector3 fireballDistance = new Vector3(2, 0, 0);
    bool mouse = true;

    [Header("GameObjects")]
    //Gets other objects that are needed
    [SerializeField] GameObject magic;
    GameObject newArrow;
    public GameObject teleportSpot;
    private GameObject teleportP;
    private Player player;
    [SerializeField] GameObject teleportParticle;

    [Header("Particle")]
    //Particles for the charging
    public ParticleSystem particle;

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
        if (canTeleport && player.classIndex == 2)
        {
            if (Input.GetKey(KeyCode.E))
            {
                GetTeleportLocation();
            }

            else if (Input.GetKeyUp(KeyCode.E))
            {
                Teleport(teleportSpot.transform.position);
                nextTeleportTime = Time.time + teleportCooldown;
            }
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(nextTeleportTime - Time.time);

        if (Time.time > nextTeleportTime)
        {
            canTeleport = true;
        }
        else canTeleport = false;

        playerGizmo = player.transform.position;
    }

    public override void Attack()
    {
        if (Time.time > nextFireTime)
        {
            player.isAttacking = true;
            player.canFlip = false;
            player.canTransform = false;

            newArrow = GameObject.Instantiate(magic, this.transform.position, Quaternion.identity);
            newArrow.GetComponent<BoxCollider2D>().enabled = false;
            particle.Play();

            
            Shoot(magic, this.gameObject, direction, newArrow);

            nextFireTime = Time.time + mageCooldown;

            player.hud.mageCooldowns[0].StartCooldown(mageCooldown);

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

    void Shoot(GameObject magic, GameObject origin, Vector3 direction, GameObject newArrow)
    {
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

    Vector3 mP;
    Vector3 outOfRangePosition;

    void GetTeleportLocation()
    {
        teleportP.SetActive(true);
        teleportSpot.SetActive(true);

        mP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mP.z = 0;

        if (mP.x < player.transform.position.x - teleportRange || mP.x > player.transform.position.x + teleportRange
            || mP.y < player.transform.position.y - teleportRange || mP.y > player.transform.position.y + teleportRange)
        {
            inRange = false;
        }
        else inRange = true;

        Collider2D collider = Physics2D.OverlapCircle(mP, 1);
        Collider2D colliderOnSpot = Physics2D.OverlapCircle(teleportSpot.transform.position, 1);

        

        if (!inRange)
        {
            Vector3 diff = mP - player.transform.position;
            diff.z = 0;
            diff = Vector3.ClampMagnitude(diff, teleportRange);

            outOfRangePosition = player.transform.position + diff;

            if (mouse)
            {
                teleportSpot.transform.position = outOfRangePosition;
            }

            if (colliderOnSpot)
            {
                teleportSpot.transform.position += upSpeed;

                if (collider)
                {
                    mouse = false;
                }
            }

            if (colliderOnSpot != true)
            {
                if (colliderOnSpot != collider)
                {
                    teleportSpot.transform.position -= downSpeed;
                }
                teleportSpot.transform.position = new Vector3(outOfRangePosition.x, teleportSpot.transform.position.y, outOfRangePosition.z);
            }

            if (collider != true)
            {
                mouse = true;
            }
        }

        if (inRange)
        {
            if (mouse)
            {
                teleportSpot.transform.position = mP;
            }

            if (colliderOnSpot)
            {
                teleportSpot.transform.position += upSpeed;

                if (collider)
                {
                    mouse = false;
                }
            }

            if (colliderOnSpot != true)
            {
                if (collider != colliderOnSpot)
                {
                    teleportSpot.transform.position -= downSpeed;
                }
                teleportSpot.transform.position = new Vector3(mP.x, teleportSpot.transform.position.y, mP.z);
            }

            if (collider != true)
            {
                mouse = true;
            }
        }

        
        teleportP.transform.position = teleportSpot.transform.position;
    }

    void Teleport(Vector3 position)
    {
        player.transform.position = position;
        teleportSpot.SetActive(false);
        teleportP.SetActive(false);
    }

    Vector3 playerGizmo = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(mP, 1);

        Gizmos.DrawLine(playerGizmo, outOfRangePosition);

        Gizmos.color = Color.blue;

        if (teleportSpot != null)
        {
            Gizmos.DrawSphere(teleportSpot.transform.position, 1);
        }

        Gizmos.DrawWireSphere(playerGizmo, teleportRange);


    }
}
