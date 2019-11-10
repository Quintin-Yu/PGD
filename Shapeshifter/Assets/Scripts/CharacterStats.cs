using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    [Header ("Healthbar")]
    public Image healthBar;

    public float maxHealth = 100;
    public float CurrentHealth { get; private set; }

    public Stats strength;
    public Stats defence;

    void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float strength)
    {
        strength -= defence.getValue();
        strength = Mathf.Clamp(strength, 0, int.MaxValue);

        CurrentHealth -= strength;
        Debug.Log(transform.name + " takes " + strength + " damage.");

        healthBar.fillAmount = CurrentHealth / maxHealth;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //This method is meant to be over writen
        Debug.Log(transform.name + " died.");
        if (gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene("GameOver");
        }
    }


}
