using UnityEngine;
using UnityEngine.UI;

/**
 * This class is used for the cooldown display in the HUD
 * 
 * @Author Quintin Yu
 */

public class Ability : MonoBehaviour
{
    public Image reloadImage;
    private float abilityCooldown;
    [SerializeField] private bool holdAbilitý;                              //This variable shouldn't be changed in other scripts but may be changed in the inspector

    public void Start()
    {
        reloadImage.fillAmount = 0;
    }

    private void Update()
    {
        if (!holdAbilitý)                                                   //If the ability is meant to be hold (Knight shield) the cooldown is filled but not used.
        {
            reloadImage.fillAmount -= Time.deltaTime / abilityCooldown;
        }
    }

    public void StartCooldown(float abilityCooldown)                        //This method is called after a attack. The parameter given with this method is the duration of the cooldown.
    {
        this.abilityCooldown = abilityCooldown;
        reloadImage.fillAmount = 1;
    }
}
