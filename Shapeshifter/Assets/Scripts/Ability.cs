using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public Image reloadImage;
    public Image[] displayImages;

    private float abilityCooldown;

    public void Start()
    {
        reloadImage.fillAmount = 0;
    }

    private void Update()
    {
        //reloadImage.fillAmount -= Time.deltaTime / abilityCooldown;
    }

    public void StartCooldown(float abilityCooldown)
    {
        this.abilityCooldown = abilityCooldown;
        reloadImage.fillAmount = 1;
    }
}
