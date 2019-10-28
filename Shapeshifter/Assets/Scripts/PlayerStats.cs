using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{

    public Text strenghtDisplay;
    public Text defenceDisplay;

    // Start is called before the first frame update
    void Start()
    {
        StrenghtDisplayUpdate();
        DefenceDisplayUpdate();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemyMelee")
        {
            Debug.Log("hit");

            TakeDamage(5);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AddDefenceUpgrades();
            DefenceDisplayUpdate();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            AddStrengthUpgrades();
            StrenghtDisplayUpdate();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(strength.getValue());
        }
    }

    void AddDefenceUpgrades()
    {
        defence.AddModifier(5);
    }

    void AddStrengthUpgrades()
    {
        strength.AddModifier(5);
    }

    void StrenghtDisplayUpdate()
    {
        strenghtDisplay.text = "You currently have: " + strength.getValue().ToString() + " Strength";
    }

    void DefenceDisplayUpdate()
    {
        defenceDisplay.text = "You currently have: " + defence.getValue().ToString() + " Defence";
    }
}
