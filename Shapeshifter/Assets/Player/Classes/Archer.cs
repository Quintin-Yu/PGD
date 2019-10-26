using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Class
{
    private float arrowForce = 1000;

    public Archer() : base(20, 600)
    {

    }

    public override void Attack(GameObject arrow, GameObject origin)
    {
        Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(origin.transform.position); ;
        direction.Normalize();

        GameObject newArrow = GameObject.Instantiate(arrow, origin.transform);
        newArrow.transform.position += direction * 0.5f;

        newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);
    }
}
