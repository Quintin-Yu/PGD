using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * A script for the classes of the player (classes as in fighter, archer, mage, cleric)
 * 
 * @Author Nicolaas Schuddeboom
 */ 

[System.Serializable]
[RequireComponent(typeof(CharacterStats))]
public class Class : CharacterStats
{
    // Declare variables
    public int speed;
    public int jumpHeight;
    public Player player;

    // An attack function to use in the classes with inheritance
    public virtual void Attack()
    {

    }

    // An ability function to use in the classes with inheritance
    public virtual void Ability()
    {

    }

    // An prewritten jump function to use in the classes with inheritance. Perhaps classes can double jump
    public virtual void Jump(Player player)
    {
        // Check if we are grounded. Then add a jump force
        if (player.groundCollider.IsGrounded)
        {
            player.rb.AddForce(jumpHeight * player.transform.up);
        }
    }

    // Take damage
    public override void TakeDamage(float strength)
    {
        // Damage
        base.TakeDamage(strength);

        // Set healthbar
        healthBar.fillAmount = CurrentHealth / maxHealth;
    }

    // Die
    public override void Die()
    {
        // Base function from CharacterStats
        base.Die();

        // Destroy player
        Destroy(gameObject);

        // Change hud
        player.hud.gameOverHUD.SetActive(true);
    }
}
