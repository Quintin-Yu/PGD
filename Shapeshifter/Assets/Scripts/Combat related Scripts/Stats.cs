using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Base class for character statistics.
 * 
 * @author Quintin Yu
 * Source: Brackeys
 */
[System.Serializable]                                       //Makes the class visible in Unity Inspector when this class is inherited
public class Stats
{
    [SerializeField]
    private float baseValue = 0;

    private List<float> modifiers = new List<float>();      //Collection of all the modifiers applied to the baseValue

    public float getValue()                                 //Get's the finalvalue after applying all the modifiers
    {
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(float modifier)                 //Adds a modifier to the modifiers array
    {
        if (modifier != 0)
        {
            modifiers.Add(modifier);
        }
    }

    public void RemoveModifier(float modifier)              //Removes a modifier from the modifiers array
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }

}
