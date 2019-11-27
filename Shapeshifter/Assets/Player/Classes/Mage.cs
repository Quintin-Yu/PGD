using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mage : Class
{
    Vector3 direction;

    [Header("Cooldown")]
    //Decides how long the cooldown is between fires
    public float mageCooldown = 5f;
    public float nextFireTime;

    //Decides how fast the fireball goes
    private float arrowForce = 1000;

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
        if (player.isAttacking)
        {
            if (player.flipped)
            {
                newArrow.transform.position = player.transform.position - fireballDistance;
            }
            else if (!player.flipped)
            {
                newArrow.transform.position = player.transform.position + fireballDistance;
            }
        }
    }

    public override void Attack()
    {
        if (Time.time > nextFireTime)
        {
            player.canFlip = false;
            player.isAttacking = true;
            player.canTransform = false;
            player.StartCoroutine(player.AttackDone(2f));

            player.speed /= 2;

            newArrow = GameObject.Instantiate(magic, this.transform.position, Quaternion.identity);

            particle.transform.position = new Vector3(newArrow.transform.position.x, newArrow.transform.position.y, -1);
            particle.Play();
            newArrow.GetComponent<BoxCollider2D>().enabled = false;

            StartCoroutine(Shoot(magic, this.gameObject, direction, newArrow, 2));
            nextFireTime = Time.time + mageCooldown;
        }
    }

    IEnumerator Shoot(GameObject magic, GameObject origin, Vector3 direction, GameObject newArrow, float time)
    {
        yield return new WaitForSeconds(time);

        direction = Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position);
        direction.Normalize();

        newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);
        newArrow.GetComponent<BoxCollider2D>().enabled = true;

        player.canTransform = true;
        player.canFlip = true;
        player.speed *= 2;

        particle.Clear();
        particle.Stop();
    }

    /*
    [Header("Line")]
    //Variables for the charging line
    public Material lineMaterial;

    public float lineWidth = 0.4f;
    public float depth = 5f;
    public float distanceFromPlayer = 2f;

    private Vector3? lineStartPoint;
    private Vector3? lineEndPoint;

    private GameObject mageChargingLine;
    private LineRenderer lineRenderer;
    */

    /*
    private void Update()
    {
        Vector3 lineDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position);
        lineDirection.Normalize();

        lineStartPoint = player.transform.position + lineDirection * distanceFromPlayer;
        lineEndPoint = GetMouseCameraPoint();

        if (lineStartPoint != null && lineEndPoint != null && lineRenderer != null)
        {
            lineRenderer.SetPositions(new Vector3[] { lineStartPoint.Value, lineEndPoint.Value });
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            mageChargingLine = new GameObject();
            lineRenderer = mageChargingLine.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lineEndPoint = null;
            Destroy(mageChargingLine);
        }
    }
    */

    /*
    private Vector3 GetMouseCameraPoint()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * depth;
    }
    */
}
