using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter : Class
{
    [SerializeField] Collider2D attack;

    public override void Attack()
    {
        StartCoroutine(GetComponent<Player>().LockMovement(0.5f));

        List<GameObject> gameObjects = attack.GetComponent<FighterAttack>().objectsInHitbox;

        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (gameObjects[i].tag == "Enemy" || gameObjects[i].tag == "EnemyMelee")
            {
                GameObject.Destroy(gameObjects[i].transform.parent.gameObject);
                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("EnemyMelee"))
        {
            TakeDamage(20);
        }
    }
}
