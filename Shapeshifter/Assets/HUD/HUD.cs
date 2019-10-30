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
    Color baseColor;


    private void Start()
    {
        knightAnimator = knightImage.GetComponent<Animator>();
        archerAnimator = archerImage.GetComponent<Animator>();
        mageAnimator = mageImage.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

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

        }else if (classIndex == 1)
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
        }


    }
}
