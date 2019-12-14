using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameCharacter
{
    public GameObject targetPlayer, healthBar;
    public float hp, maxRange, hpTimer;

    public virtual void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        hpTimer = 2f;
    }

    public virtual void FixedUpdate()
    {
        DeactivateHPBar();
    }

    public void DeactivateHPBar()
    {
        hpTimer -= Time.deltaTime;
        if (hpTimer <= 0)
        {
            healthBar.SetActive(false);
        }
    }
}
