using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mage : Class
{
    public float mageCooldown = 5f;
    public float nextFireTime;

    private float arrowForce = 1000;

    [SerializeField] GameObject magic;
    private Player player;

    public ParticleSystem particle;

    private void Start()
    {
        player = GetComponent<Player>();
        particle = GameObject.FindObjectOfType<ParticleSystem>();
        particle.Stop();
    }

    public override void Attack()
    {
        if (Time.time > nextFireTime)
        {
            player.isAttacking = true;
            player.canTransform = false;
            player.StartCoroutine(player.LockMovement(2f));
            player.StartCoroutine(player.AttackDone(2f));
            Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position); ;
            direction.Normalize();

            GameObject newArrow = GameObject.Instantiate(magic, this.transform.position, Quaternion.identity);
            newArrow.transform.position += direction * 0.5f;

            particle.transform.position = new Vector3(newArrow.transform.position.x, newArrow.transform.position.y, -1);
            particle.Play();
            newArrow.GetComponent<BoxCollider2D>().enabled = false;

            StartCoroutine(Shoot(magic, this.gameObject, direction, newArrow, 2));
            FindObjectOfType<AudioManager>().Play("Fireball Cast");
            nextFireTime = Time.time + mageCooldown;
        }
    }

    IEnumerator Shoot(GameObject magic, GameObject origin, Vector3 direction, GameObject newArrow, float time)
    {
        yield return new WaitForSeconds(time);
        
        newArrow.GetComponent<Rigidbody2D>().AddForce(direction * arrowForce);
        newArrow.GetComponent<BoxCollider2D>().enabled = true;
        player.canTransform = true;
        particle.Clear();
        particle.Stop();
    }
}
