using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Class
{
    [SerializeField] GameObject arrow;
    [SerializeField] float arrowForce = 1000;

    public override void Attack()
    {
        // Get direction for arrow
        Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position); ;
        direction.Normalize();

        // Create arrow
        GameObject newArrow = GameObject.Instantiate(arrow, this.transform.position, Quaternion.identity);
        newArrow.transform.position += direction * 0.5f;

        // Add force
        newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);

    }
}
