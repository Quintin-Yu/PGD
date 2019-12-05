using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter : Class
{
    [SerializeField] Collider2D attack;

    CharacterStats myStats;
    public GameObject enemyHPBar;
    bool delayfinished = false;

    public int shieldDefence;

    void Start()
    {
        shieldDefence = 6;
        //myStats = GetComponent<CharacterStats>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("EnemyArrow"))
        {
            TakeDamage(15);
        }
    }

    public override void Attack()
    {
        GetComponent<Player>().isAttacking = true;
        StartCoroutine(GetComponent<Player>().LockMovement(0.5f));
        StartCoroutine(GetComponent<Player>().AttackDone(0.5f));

        //FindObjectOfType<AudioManager>().Play("Miss Melee");

        List<GameObject> gameObjects = attack.GetComponent<FighterAttack>().objectsInHitbox;

        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (gameObjects[i].tag == "Enemy" || gameObjects[i].tag == "EnemyMelee")
            {

                //GameObject.Destroy(gameObjects[i].transform.parent.gameObject);
                CombatController enemyCombat = gameObjects[i].transform.parent.GetComponent<CombatController>();
                myStats = gameObjects[i].transform.parent.GetComponent<CharacterStats>();

                if (enemyCombat != null)
                {
                    Debug.Log(enemyCombat + " " + myStats);

                    this.GetComponent<CombatController>().Attack(myStats);

                    try
                    {
                        if (gameObjects[i] != null)
                        {
                            gameObjects[i].transform.parent.GetComponent<EnemyMelee>().healthBar.SetActive(true);
                            gameObjects[i].transform.parent.GetComponent<EnemyMelee>().hpTimer = 2;
                        }
                    }
                    catch
                    {

                    }
                }

                //FindObjectOfType<AudioManager>().Play("Hit Melee");
                //GameObject.Destroy(gameObjects[i].transform.parent.gameObject);
                 //CombatController enemyCombat = gameObjects[i].transform.parent.GetComponent<CombatController>();
                 myStats = gameObjects[i].transform.parent.GetComponent<CharacterStats>();

                 if (enemyCombat != null)
                 {
                     Debug.Log(enemyCombat + " " + myStats);
                     this.GetComponent<CombatController>().Attack(myStats);
                 }
                 

                return;
            }
        }
    }

    public void Shield()
    {
        myStats.defence.AddModifier(shieldDefence);
    }

    public void StopBlocking()
    {
        myStats.defence.RemoveModifier(shieldDefence);
    }


}
