using UnityEngine;
using UnityEngine.UI;

/**
 * This class is placed on each character that has stats to fight
 * This class is to be used as a parent class for other stat scripts.
 * 
 * @author Quintin Yu
 * Source: Brackeys
 */

public class CharacterStats : MonoBehaviour
{
    [Header ("Healthbar")]                                              //Shows a header in the Inspector of Unity that the variables are for the healthbar
    public Image healthBar;
    public Text healthText;

    [Header("Stats")]                                                   //Shows a header in the Inspector of Unity that the variables are for the stats of each character
    public float maxHealth = 100;
    public float CurrentHealth { get; private set; }

    public Stats strength;
    public Stats defence;
    public Stats attackSpeed;

    void Awake()
    {
        CurrentHealth = maxHealth;
        SetHealthText();
    }

    public virtual void TakeDamage(float strength)                      //Used to calculate the damage a character has to take based on the strenght value of the enemy character minus the defence of the character
    {
        strength -= defence.getValue();
        strength = Mathf.Clamp(strength, 0, int.MaxValue);              //When the defence is higher than the strenght of the enemy the damage gets set to 0 instead of healing the character;

        CurrentHealth -= strength;
        healthBar.fillAmount = CurrentHealth / maxHealth;               //Sets the healthbar
        SetHealthText();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()                                           //This method is meant to be changed. By overriding this method you can give each character a different death animation
    {
        //This method is meant to be over writen
    }

    public void SetHealthText()                                         //This method places a text over the healthbar
    {
        healthText.text = "HP: " + CurrentHealth + " / " + maxHealth;
    }
}
