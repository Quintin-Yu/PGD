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

    [Header("GameObjects")]
    //Gets other objects that are needed
    [SerializeField] GameObject magic;
    private Player player;

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

    [Header("Particle")]
    //Particles for the charging
    public ParticleSystem particle;

    private void Start()
    {
        player = GetComponent<Player>();
        particle = GameObject.FindObjectOfType<ParticleSystem>();
        particle.Stop();
    }

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

    public override void Attack()
    {
        if (Time.time > nextFireTime)
        {
            player.isAttacking = true;
            player.canTransform = false;
            player.StartCoroutine(player.LockMovement(2f));
            player.StartCoroutine(player.AttackDone(2f));

            GameObject newArrow = GameObject.Instantiate(magic, this.transform.position, Quaternion.identity);
            newArrow.transform.position += direction * 0.5f;

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

        particle.Clear();
        particle.Stop();
    }

    /*
    private Vector3 GetMouseCameraPoint()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * depth;
    }
    */
}
