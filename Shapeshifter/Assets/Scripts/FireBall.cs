using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public GameObject explosion;
    GameObject newExplosion;
    ParticleSystem explosionParticles;
    bool hasExploded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag.Equals("EnemyMelee"))
        {
            Debug.Log("hit");
            GetComponent<BoxCollider2D>().enabled = false;

            if (!hasExploded)
            {
                StartCoroutine(Explosion(0.5f));
            }
            Destroy(other.transform.parent.gameObject);
        }

        if (other.gameObject.tag == "MeleeDummy")
        {
            GetComponent<BoxCollider2D>().enabled = false;

            if (!hasExploded)
            {
                StartCoroutine(Explosion(0.5f));
            }
            Destroy(other.transform.parent.gameObject);
        }

        if (other.gameObject.tag == "BreakableWall")
        {
            GetComponent<BoxCollider2D>().enabled = false;

            if (!hasExploded)
            {
                StartCoroutine(Explosion(0.5f));
            }
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag.Equals("map"))
        {
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

