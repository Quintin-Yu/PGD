using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    
    public bool grounded = false;

    public bool IsGrounded
    {
        get { return grounded; }
        set { grounded = value; }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        IsGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsGrounded = false;
    }

    /*void FixedUpdate() {
        if (grounded)
        {
            animator.SetBool("isgrounded", true);
        }
        else
        {
            animator.SetBool("isgrounded", false);
        }
    }*/
}
