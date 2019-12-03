using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameCharacter
{
    public GameObject targetPlayer;
    public float hp;
    public float maxRange;

    public virtual void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual void FixedUpdate()
    {

    }
}
