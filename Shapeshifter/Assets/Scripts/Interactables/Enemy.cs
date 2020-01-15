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
    [Header("Gameobjects")]
    public GameObject targetPlayer;
    public GameObject healthBar;

    [Header("Stats and values")]
    public float hp;
    public float maxRange;
    public float hpTimer;

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
