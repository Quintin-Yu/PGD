﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public Image image;
    private float abilityCooldown;

    public void Start()
    {
        image.fillAmount = 0;
    }

    private void Update()
    {
        image.fillAmount -= Time.deltaTime / abilityCooldown;
    }

    public void StartCooldown(float abilityCooldown)
    {
        this.abilityCooldown = abilityCooldown;
        image.fillAmount = 1;
    }
}
