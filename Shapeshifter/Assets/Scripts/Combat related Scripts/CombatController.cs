using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is called for the combat.
 * 
 * @author Quintin Yu
 * Source: Brackeys
 */

[RequireComponent(typeof(CharacterStats))]
public class CombatController : MonoBehaviour
{
    public CharacterStats mystats;                                  //Declares the stats from the attacking character

    public void Start()
    {
        mystats = GetComponent<CharacterStats>();                   //Initializes the stats from the attacking character
    }

    public void Attack(CharacterStats targetStats)                  //This method needs a parameter CharacterStats. This script has to be the enemies script
    {
        targetStats.TakeDamage(mystats.strength.getValue());        //Calls the method TakeDamage of the character that got attacked using the strenght value of the attacking character
    }
}
