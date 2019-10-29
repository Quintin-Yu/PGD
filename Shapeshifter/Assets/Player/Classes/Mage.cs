using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Class
{
    private float arrowForce = 1000;

    public Mage() : base(14, 500)
    {

    }

    public override void Attack(GameObject magic, GameObject origin)
    {
        Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(origin.transform.position); ;
        direction.Normalize();

        GameObject newArrow = GameObject.Instantiate(magic, origin.transform.position, Quaternion.identity);
        newArrow.transform.position += direction * 0.5f;

        newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);
    }
}
