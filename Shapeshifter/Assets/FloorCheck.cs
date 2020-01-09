using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCheck : MonoBehaviour
{
    public bool isHitting = false;
    int collisionCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionCount--;
    }

    private void FixedUpdate()
    {
        if (collisionCount == 0)
        {
            isHitting = false;
        }
        else
        {
            isHitting = true;
        }
    }
}
