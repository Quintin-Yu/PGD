using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image knightImage;
    public Image archerImage;
    public Image mageImage;
    Animator knightAnimator;
    Animator archerAnimator;
    Animator mageAnimator;

    public Image[] reload;

    Color baseColor;

    /// <summary>
    /// Call to the ability slots which have cooldown.
    /// basicAbility is for the normalt attacks each class has, for the warrior this is his sword strike, archer bow shot and mage his fireball.
    /// eAbility is the basic ability each class has. For the warrior this is a charge, allowing him to dash forward and hurting all enemies in it's path.
    /// </summary>
    public Ability basicAbility;
    public Ability eAbility;

    private void Start()
    {
        knightAnimator = knightImage.GetComponent<Animator>();
        archerAnimator = archerImage.GetComponent<Animator>();
        mageAnimator = mageImage.GetComponent<Animator>();

        foreach (Image cd in reload)
        {
            cd.fillAmount = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ShapeShiftCooldownDisplay(3);
    }

    public void playAnimation(int classIndex)
    {
        if (classIndex == 0)
        {
            knightAnimator.SetBool("knightSwitch", true);
            mageAnimator.SetBool("mageSwitch", false);
            archerAnimator.SetBool("archerSwitch", false);

            baseColor = knightImage.color;
            baseColor.a = 1f;
            knightImage.color = baseColor;

            baseColor = mageImage.color;
            baseColor.a = 0.5f;
            mageImage.color = baseColor;

            baseColor = archerImage.color;
            baseColor.a = 0.5f;
            archerImage.color = baseColor;

            basicAbility.displayImages[0].gameObject.SetActive(true);
            basicAbility.displayImages[1].gameObject.SetActive(false);
            basicAbility.displayImages[2].gameObject.SetActive(false);

            StartCooldown();
        }
        else if (classIndex == 1)
        {
            archerAnimator.SetBool("archerSwitch", true);
            knightAnimator.SetBool("knightSwitch", false);
            mageAnimator.SetBool("mageSwitch", false);

            baseColor = knightImage.color;
            baseColor.a = 0.5f;
            knightImage.color = baseColor;

            baseColor = mageImage.color;
            baseColor.a = 0.5f;
            mageImage.color = baseColor;

            baseColor = archerImage.color;
            baseColor.a = 1f;
            archerImage.color = baseColor;

            basicAbility.displayImages[0].gameObject.SetActive(false);
            basicAbility.displayImages[1].gameObject.SetActive(true);
            basicAbility.displayImages[2].gameObject.SetActive(false);

            StartCooldown();
        }
        else if (classIndex == 2)
        {
            knightAnimator.SetBool("knightSwitch", false);
            mageAnimator.SetBool("mageSwitch", true);
            archerAnimator.SetBool("archerSwitch", false);

            baseColor = knightImage.color;
            baseColor.a = 0.5f;
            knightImage.color = baseColor;

            baseColor = mageImage.color;
            baseColor.a = 01f;
            mageImage.color = baseColor;

            baseColor = archerImage.color;
            baseColor.a = 0.5f;
            archerImage.color = baseColor;

            basicAbility.displayImages[0].gameObject.SetActive(false);
            basicAbility.displayImages[1].gameObject.SetActive(false);
            basicAbility.displayImages[2].gameObject.SetActive(true);

            StartCooldown();
        }
    }

    //This method sets all the cooldown displays to full.
    private void StartCooldown()
    {
        foreach(Image cd in reload)
        {
            cd.fillAmount = 1;
        }
    }

    public void ShapeShiftCooldownDisplay(float time)
    {
        //The number after the devision has to be changed when you change the transform time in the Player class.
        foreach (Image cd in reload)
        {
            cd.fillAmount -= Time.deltaTime / time;
        }
    }
}
