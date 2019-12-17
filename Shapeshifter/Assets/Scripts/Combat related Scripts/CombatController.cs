using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CombatController : MonoBehaviour
{
    public CharacterStats mystats;

    public void Start()
    {
        mystats = GetComponent<CharacterStats>();
    }

    public void Attack (CharacterStats targetStats)
    {
        Debug.Log(mystats.strength.getValue());

        targetStats.TakeDamage(mystats.strength.getValue());
        Debug.Log(targetStats + " Has taken damage");
    }
}
