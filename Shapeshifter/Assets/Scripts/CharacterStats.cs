using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth { get; private set; }

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
