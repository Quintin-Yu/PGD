using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [Header ("Healthbar")]
    public Image healthBar;

    public float maxHealth = 100;
    public float currentHealth { get; private set; }

    public Stats strength;
    public Stats defence;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int strength)
    {
        strength -= defence.getValue();
        strength = Mathf.Clamp(strength, 0, int.MaxValue);

        currentHealth -= strength;
        Debug.Log(transform.name + " takes " + strength + " damage.");

        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
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
