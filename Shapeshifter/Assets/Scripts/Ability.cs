using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public Image reloadImage;
    private float abilityCooldown;
    [SerializeField] private bool holdAbilitý;

    public void Start()
    {
        reloadImage.fillAmount = 0;
    }

    private void Update()
    {
        if (!holdAbilitý)
        {
            reloadImage.fillAmount -= Time.deltaTime / abilityCooldown;
        }
    }

    public void StartCooldown(float abilityCooldown)
    {
        this.abilityCooldown = abilityCooldown;
        reloadImage.fillAmount = 1;
    }
}
