﻿using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [Header ("Healthbar")]
    public Image healthBar;

    public float maxHealth = 100;
    public float CurrentHealth { get; private set; }

    public Stats strength;
    public Stats defence;
    public Stats attackSpeed;

    void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public virtual void TakeDamage(float strength)
    {
        strength -= defence.getValue();
        strength = Mathf.Clamp(strength, 0, int.MaxValue);

        CurrentHealth -= strength;
        Debug.Log(transform.name + " takes " + strength + " damage.");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //This method is meant to be over writen
        Debug.Log(transform.name + " died.");
    }


}
