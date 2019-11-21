using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyArrow : MonoBehaviour
{
    public float arrowLifeDuration = 10;

    private void Update()
    {
        arrowLifeDuration -= Time.deltaTime;

        if (arrowLifeDuration <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
