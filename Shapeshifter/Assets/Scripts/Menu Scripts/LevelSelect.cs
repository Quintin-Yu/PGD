using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void WarriorLevel()
    {
        SceneManager.LoadScene("FighterLevel");
    }

    public void MageLevel()
    {
        SceneManager.LoadScene("MageLevel");
    }

    public void ArcherLevl()
    {
        SceneManager.LoadScene("ArcherLevel");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
