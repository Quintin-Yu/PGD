using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LevelSelector()
    {
        SceneManager.LoadScene("Level Selector");
    }

    public void FighterLevel()
    {
        SceneManager.LoadScene("FighterLevel");
    }

    public void MageLevel()
    {
        SceneManager.LoadScene("MageLevel");
    }

    public void ArcherLevel()
    {
        SceneManager.LoadScene("ArcherLevel");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Gane Over");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
