using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is used to switch between scens. Place this script on a eventmanager
 * 
 * @author Quintin Yu
 */

public class SceneManagement : MonoBehaviour
{
    public void LevelSelector()
    {
        SceneManager.LoadScene("Level Selector");
    }

    public void FighterLevel()
    {
        SceneManager.LoadScene("FighterLevel");
        Time.timeScale = 1f;
    }

    public void MageLevel()
    {
        SceneManager.LoadScene("MageLevel");
        Time.timeScale = 1f;
    }

    public void ArcherLevel()
    {
        SceneManager.LoadScene("ArcherLevel");
        Time.timeScale = 1f;
    }

    public void StartLevel1()
    {
        SceneManager.LoadScene("CoreMechanicLevel");
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Gane Over");
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
