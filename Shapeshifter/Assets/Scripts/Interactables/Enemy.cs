using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is the main class of the enemy.
 * This class is a child of the GameCharacter class and a parent for every enemy type
 * 
 */

public class Enemy : GameCharacter
{
    public GameObject targetPlayer, healthBar;
    public float hp, maxRange, hpTimer;

    public virtual void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");      //Sets the target of every enemy as the player
        hpTimer = 2f;
    }

    public virtual void FixedUpdate()
    {
        DeactivateHPBar();
    }

    public void DeactivateHPBar()                                       //Deactivates the healthbar of the enemy after a set amount of time.
    {
        hpTimer -= Time.deltaTime;
        if (hpTimer <= 0)
        {
            healthBar.SetActive(false);
        }
    }
}
