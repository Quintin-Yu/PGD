using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mage : Class
{
    Vector3 direction;

    [Header("Cooldown")]
    //Decides how long the cooldown is between fires
    public float mageCooldown = 1f;
    public float nextFireTime;

    //Decides how fast the fireball goes
    private float arrowForce = 1000;
    private float charge = 0;

    private bool charging = false;
    private bool lClick = false;
    private bool attack = false;
    private Vector3 fireballDistance = new Vector3(2, 0, 0);

    [Header("GameObjects")]
    //Gets other objects that are needed
    [SerializeField] GameObject magic;
    GameObject newArrow;
    private Player player;

    [Header("Particle")]
    //Particles for the charging
    public ParticleSystem particle;

    private void Start()
    {
        player = GetComponent<Player>();
        particle = GameObject.FindObjectOfType<ParticleSystem>();
        particle.Stop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Teleport(0);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            Teleport(1);
        }
    }

    private void FixedUpdate()
    {

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


    void Teleport(int way)
    {
        if (way == 0)
        {
            
        }
    }
}
