using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter : Class
{
    public Fighter() : base(10, 300)
    {

    }

    public override void Attack(GameObject attackHitbox, GameObject player)
    {
        List<GameObject> gameObjects = attackHitbox.GetComponent<FighterAttack>().objectsInHitbox;

        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (gameObjects[i].tag == "Enemy" || gameObjects[i].tag == "EnemyMelee")
            {
                GameObject.Destroy(gameObjects[i].transform.parent.gameObject);
                return;
            }
        }
    }
}
