﻿using System.Collections;
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
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("hit");
            GetComponent<BoxCollider2D>().enabled = false;

            if (!hasExploded)
            {
                StartCoroutine(Explosion(0.5f));
            }
            Destroy(other.transform.parent.gameObject);
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
        explosionParticles.Play();

        yield return new WaitForSeconds(time);        

        explosionParticles.Stop();
        Destroy(newExplosion.gameObject);
        Destroy(gameObject);
    }

}

