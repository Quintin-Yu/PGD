using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable_Rest : MonoBehaviour
{
    public EnemyRanged[] enemyRangeds;
    public Enemy[] meleeEnemies;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemyRangeds.Length; i++)
        {
            enemyRangeds[i].enabled = false;
        }

        for (int i = 0; i < meleeEnemies.Length; i++)
        {
            meleeEnemies[i].enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Sup");

            for (int i = 0; i < enemyRangeds.Length; i++)
            {
                enemyRangeds[i].enabled = true;
            }

            for (int i = 0; i < meleeEnemies.Length; i++)
            {
                meleeEnemies[i].enabled = true;
            }
        }
    }
}
