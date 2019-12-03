using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAttack : MonoBehaviour
{
    public List<GameObject> objectsInHitbox = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectsInHitbox.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsInHitbox.Remove(collision.gameObject);
    }
}
