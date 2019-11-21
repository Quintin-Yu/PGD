using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public Rigidbody2D rb;
    public GroundCollider groundCollider;
    GameObject player;
    public GameObject enemyArrow;

    float speed;
    int jumpHeight;
    float hp;
    float arrowForce = 1000f;
    float fireRate;
    float nextFire;

    public float maxRange;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        speed = 0.1f;
        hp = 0.1f;
        fireRate = 3f;
        nextFire = 0.0f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Time.time > nextFire)
        {
            
            Vector3 direction = (player.transform.position - transform.position).normalized * arrowForce;
            direction.Normalize();

            if (player.transform.position.x - rb.transform.position.x >= -maxRange && player.transform.position.x - rb.transform.position.x <= 0)
            {
                GameObject newArrow = Instantiate(enemyArrow, transform.position, Quaternion.identity);
                newArrow.transform.position += direction * 0.5f;
                newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);

            }

            else if (rb.transform.position.x - player.transform.position.x >= -maxRange && rb.transform.position.x - player.transform.position.x <= 0)
            {
                GameObject newArrow = Instantiate(enemyArrow, transform.position, Quaternion.identity);
                newArrow.transform.position += direction * 0.5f;
                newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);
            }
            nextFire = Time.time + fireRate;
        }
        
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(1);
    }
}
