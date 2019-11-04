using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject options;

    public void PlayGame()
    {
        //SceneManager.LoadScene("");
    }

    public void TestLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        //options.SetActive(true);
    }
}
