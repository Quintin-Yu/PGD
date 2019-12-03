using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Class
{
    public GameObject arrow;                //Get's the projectile the player archer has to shoot.
    public float arrowForce;                //The speed the arrow shoots.

    private void Start()
    {
        arrowForce = 1000;
    }

//Archers attack.
    public override void Attack()
    {
        GetComponent<Player>().isAttacking = true;

        StartCoroutine(GetComponent<Player>().AttackDone(1f));

        // Get direction for arrow
        Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position);
        direction.Normalize();

        // Create arrow
        GameObject newArrow = GameObject.Instantiate(arrow, this.transform.position, Quaternion.identity);
        newArrow.transform.position += direction * 0.5f;

        // Add force
        newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);

        FindObjectOfType<AudioManager>().Play("Shoot Bow");
    }
}
