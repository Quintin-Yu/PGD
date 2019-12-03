﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour
{
    public Rigidbody2D rb;
    public GroundCollider groundCollider;
    public float speed;
    public float jumpHeight;
    public bool recentlyAttacked;

    public IEnumerator AttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);

        recentlyAttacked = false;
    }
}
