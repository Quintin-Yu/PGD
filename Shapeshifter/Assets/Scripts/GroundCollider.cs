using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script check if the player is grounded
 * 
 * @Author Nicolaas Schuddeboom
 */ 

public class GroundCollider : MonoBehaviour
{
    // Is the player grounded variable
    public bool grounded = false;

    // Getter and setter
    public bool IsGrounded
    {
        get { return grounded; }
        set { grounded = value; }
    }

    // If we hit something, we can jump
    void OnCollisionEnter2D(Collision2D collision)
    {
        IsGrounded = true;
    }

    // If we leave something, we can't jump
    private void OnCollisionExit2D(Collision2D collision)
    {
        IsGrounded = false;
    }
}
