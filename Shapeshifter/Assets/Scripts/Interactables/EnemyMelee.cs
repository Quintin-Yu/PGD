using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
{
    public float damage = 5f;

    public float attackSpeedReload;
    private float attackSpeedReset;

    CharacterStats myStats;

    // Start is called before the first frame update
    public override void Start()
    {
        speed = 0.1f;

        hp = 1;
        maxRange = 40;

        rb = GetComponent<Rigidbody2D>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player");

        attackSpeedReload = 2;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (targetPlayer.transform.position.x - rb.transform.position.x >= -maxRange && targetPlayer.transform.position.x - rb.transform.position.x <= 0)
        {
            transform.Translate(-speed, 0f, 0f);
        }
        else if (rb.transform.position.x - targetPlayer.transform.position.x >= -maxRange && rb.transform.position.x - targetPlayer.transform.position.x <= 0)
        {
            transform.Translate(speed, 0f, 0f);
        }

        if (attackSpeedReset > 0)
        {
            attackSpeedReset -= Time.deltaTime;
        }
    }
    
    public void OnCollisionStay2D(Collision2D collision)
    {
        if(attackSpeedReset <= 0) {
            if (collision.gameObject.tag.Equals("Player"))
            {
                CombatController playerCombat = collision.gameObject.GetComponent<CombatController>();
                myStats = collision.transform.GetComponent<CharacterStats>();

                if (playerCombat != null)
                {
                    this.GetComponent<CombatController>().Attack(myStats);
                    Debug.Log(playerCombat + " || " + myStats);
                    attackSpeedReset = attackSpeedReload;
                }
            }
        }
    } 
}
