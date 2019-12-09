using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable_Rest : MonoBehaviour
{
    public Enemy[] Enemies;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < Enemies.Length; i++)
        {
            Enemies[i].enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            for (int i = 0; i < Enemies.Length; i++)
            {
                Enemies[i].enabled = true;
            }
        }
    }
}
