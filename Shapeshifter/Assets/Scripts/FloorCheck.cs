using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class checks if the collider is hitting something.
 * This class is used so the enemies won't fall from platforms and instead see that there isn't any collision so they will walk back
 * @author Beau Eben
 */

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
