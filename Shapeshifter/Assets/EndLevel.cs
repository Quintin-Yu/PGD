using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private HUD hud;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        hud.WinScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
