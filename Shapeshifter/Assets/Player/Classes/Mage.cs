using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mage : Class
{
    public float mageCooldown = 5f;
    public float nextFireTime;

    private float arrowForce = 1000;

    private GameObject player;
    private MonoBehaviour _mb;

    public ParticleSystem particle;

    public Mage() : base(14, 500)
    {
        _mb = GameObject.FindObjectOfType<MonoBehaviour>();
        player = GameObject.FindGameObjectWithTag("Player");
        particle = GameObject.FindObjectOfType<ParticleSystem>();
        particle.Stop();
    }

    public override void Attack(GameObject magic, GameObject origin)
    {
        if (Time.time > nextFireTime)
        {
            player.GetComponent<Player>().canTransform = false;
            player.GetComponent<Player>().StartCoroutine(player.GetComponent<Player>().LockMovement(2f));
            Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(origin.transform.position); ;
            direction.Normalize();

            GameObject newArrow = GameObject.Instantiate(magic, origin.transform.position, Quaternion.identity);
            newArrow.transform.position += direction * 0.5f;

            particle.transform.position = new Vector3(newArrow.transform.position.x, newArrow.transform.position.y, -1);
            particle.Play();
            newArrow.GetComponent<BoxCollider2D>().enabled = false;

            _mb.StartCoroutine(Shoot(magic, origin, direction, newArrow, 2));
            nextFireTime = Time.time + mageCooldown;
        }
    }

    IEnumerator Shoot(GameObject magic, GameObject origin, Vector3 direction, GameObject newArrow, float time)
    {
        yield return new WaitForSeconds(time);
        
        newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);
        newArrow.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<Player>().canTransform = true;
        particle.Clear();
        particle.Stop();
    }
}
