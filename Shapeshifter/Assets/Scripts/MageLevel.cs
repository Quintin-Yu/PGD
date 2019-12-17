using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MageLevel : MonoBehaviour
{
    public Enemy[] enemiesNeedToKill;
    public Enemy[] enemies;
    public Player player;
    public GameObject wall;

    public CinemachineVirtualCamera vcam;
    public Camera c;

    private bool isEnabled;
    private bool zoomOut = false;

    private void Start()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].enabled = false;
        }

        c = Camera.main;

        wall.GetComponent<BoxCollider2D>().enabled = false;
        wall.GetComponent<SpriteRenderer>().enabled = false;

        player.classIndex = 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemiesNeedToKill[0] == null && enemiesNeedToKill[1] == null)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    enemies[i].enabled = true;
                }
            }

            if (vcam.m_Lens.OrthographicSize > 18f)
            {
                LookAtEnemy();
            }

            if (vcam.m_Lens.OrthographicSize < 20f && !zoomOut)
            {
                vcam.m_Lens.OrthographicSize += 0.1f;
            }

            if (vcam.m_Lens.OrthographicSize > 10 && zoomOut)
            {
                vcam.m_Lens.OrthographicSize -= 0.1f;
            }

            if (zoomOut)
            {
                wall.GetComponent<BoxCollider2D>().enabled = false;
                wall.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (!zoomOut)
            {
                wall.GetComponent<BoxCollider2D>().enabled = true;
                wall.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    void LookAtEnemy()
    {
        int enemieAtNow = 0;

        if (enemies[enemieAtNow] == null)
        {
            enemieAtNow += 1;
        }

        if (enemies[enemieAtNow] != null)
        {
            vcam.LookAt = enemies[enemieAtNow].transform;
        }else if (enemies[enemieAtNow] == null)
        {
            zoomOut = true;
        }
    }
}
