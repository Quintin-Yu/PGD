using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyArrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag.Equals("map"))
        {
            Destroy(gameObject);
        }
    }
}
