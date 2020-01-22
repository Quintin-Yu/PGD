using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectiles
{

    public GameObject explosion;
    GameObject newExplosion;
    ParticleSystem explosionParticles;
    bool hasExploded = false;
    bool canDestroy = false;
    public float range = 2f;
    float destroyTime;
    bool canCount = true;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        projectileLifeTime = 0.1f;

        destroyTime = Time.time + range;
    }

    private void Update()
    {
        if (canDestroy)
        {
            Destroy(gameObject);
        }

        if (canCount)
        {
            if (Time.time > destroyTime)
            {
                canDestroy = true;
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag.Equals("EnemyMelee") || other.gameObject.tag.Equals("EnemyRanged") || other.gameObject.tag == "MeleeDummy")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            canCount = false;

            if (!hasExploded)
            {
                StartCoroutine(Explosion(0.5f));
            }
        }

        if (other.gameObject.tag == "Breakable")
        {
            GetComponent<BoxCollider2D>().enabled = false;

            canCount = false;

            if (!hasExploded)
            {
                StartCoroutine(Explosion(0.5f));
            }
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag.Equals("map"))
        {
            canCount = false;

            GetComponent<BoxCollider2D>().enabled = false;
            if (!hasExploded)
            {
                StartCoroutine(Explosion(0.5f));
            }
        }
    }

    IEnumerator Explosion(float time)
    {
        hasExploded = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        newExplosion = GameObject.Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        explosionParticles = explosion.GetComponentInChildren<ParticleSystem>();
        explosionParticles.transform.position = new Vector3(explosionParticles.transform.position.x, explosionParticles.transform.position.y, -0.05f);
        explosionParticles.Play();
        FindObjectOfType<AudioManager>().Play("Explosion");

        yield return new WaitForSeconds(time);        

        explosionParticles.Stop();
        Destroy(newExplosion.gameObject);
        Destroy(gameObject);
    }

}

