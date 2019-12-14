﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Projectiles : MonoBehaviour
{
    public float projectileLifeTime;

    private void Update()
    {
        projectileLifeTime -= Time.deltaTime;

        if (projectileLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag.Equals("map"))
        {
            Destroy(gameObject);
        }
    }
}
