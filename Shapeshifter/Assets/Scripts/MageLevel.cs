using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MageLevel : MonoBehaviour
{
    public Enemy[] enemiesNeedToKill;
    public Enemy[] enemies;
    public EnemyRanged enemieRanged;

    public CinemachineVirtualCamera vcam;

    private bool isEnabled;

    private void Start()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].enabled = false;
        }

        enemieRanged.enabled = false;
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
            if (enemieRanged != null)
            {
                enemieRanged.enabled = true;
            }

            if (vcam.m_Lens.OrthographicSize >= 15)
            {
                LookAtEnemy();
            }

            if (vcam.m_Lens.OrthographicSize < 20f)
            {
                vcam.m_Lens.OrthographicSize += 0.1f;
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

        if (enemieRanged != null)
        {
            vcam.LookAt = enemieRanged.transform;
        }
        else if(enemies[enemieAtNow] != null)
        {
            vcam.LookAt = enemies[enemieAtNow].transform;
        }
        else
        {
            if (vcam.m_Lens.OrthographicSize >= 10)
            {
                vcam.m_Lens.OrthographicSize -= 0.1f;
            }
        }
    }
}
