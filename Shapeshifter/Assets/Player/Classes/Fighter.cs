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
        player.GetComponent<Player>().StartCoroutine(player.GetComponent<Player>().LockMovement(0.5f));

        List<GameObject> gameObjects = attackHitbox.GetComponent<FighterAttack>().objectsInHitbox;

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.tag == "Enemy")
            {
                Debug.Log("HIT!");
            }
        }
    }
}
