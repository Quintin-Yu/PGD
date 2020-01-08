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

    /**
     * All class huds are gathered here as gameobjects so these can be enabled and disabled based on the class the player is currently playing
     * 
     * The Abilities are an array so different types of abilities can be easily added to the game
     */
    [SerializeField] private GameObject knightHUD;
    [SerializeField] private GameObject archerHUD;
    [SerializeField] private GameObject mageHUD;
    public GameObject gameOverHUD;
    public GameObject Pause;

    public Ability[] knightCooldowns;
    public Ability[] archerCooldowns;
    public Ability[] mageCooldowns;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause.SetActive(true);
            Time.timeScale = 0f;
        }
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

            knightHUD.SetActive(true);
            archerHUD.SetActive(false);
            mageHUD.SetActive(false);

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

            knightHUD.SetActive(false);
            archerHUD.SetActive(true);
            mageHUD.SetActive(false);

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

            knightHUD.SetActive(false);
            archerHUD.SetActive(false);
            mageHUD.SetActive(true);

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

    public void ResumeGame()
    {
        Pause.SetActive(false);
        Time.timeScale = 1f;
    }
}
