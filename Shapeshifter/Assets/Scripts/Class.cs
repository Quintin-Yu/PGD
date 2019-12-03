﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
[RequireComponent(typeof(CharacterStats))]
public class Class : CharacterStats
{
    public int speed;
    public int jumpHeight;

    public virtual void Attack()
    {

    }

    public virtual void Jump(Player player)
    {
        if (player.groundCollider.IsGrounded)
        {
            player.rb.AddForce(jumpHeight * player.transform.up);
        }
    }

    public override void TakeDamage(float strength)
    {
        base.TakeDamage(strength);

        healthBar.fillAmount = CurrentHealth / maxHealth;
    }

    public override void Die()
    {
        base.Die();

        SceneManager.LoadScene("GameOver");
    }
}