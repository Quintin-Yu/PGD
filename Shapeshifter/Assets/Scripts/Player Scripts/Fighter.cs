using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter : Class
{
    [SerializeField] Collider2D attack;

    CharacterStats myStats, playerStats;
    public GameObject enemyHPBar;
    bool delayfinished = false;

    public int shieldDefence;
    public int knockback = 100;

    void Start()
    {
        shieldDefence = 6;
        playerStats = GetComponent<CharacterStats>();
        myStats = GetComponent<CharacterStats>();
        
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
        if (!delayfinished)
        {
            delayfinished = true;
            StartCoroutine(MeleeAttack(0.3f));

            GetComponent<Player>().isAttacking = true;
            StartCoroutine(GetComponent<Player>().LockMovement(0.5f));
            StartCoroutine(GetComponent<Player>().AttackDone(0.5f));

            FindObjectOfType<AudioManager>().Play("Miss Melee");

            List<GameObject> gameObjects = attack.GetComponent<FighterAttack>().objectsInHitbox;

            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (gameObjects[i].tag == "EnemyRanged" || gameObjects[i].tag == "EnemyMelee")
                {
                    //GameObject.Destroy(gameObjects[i].transform.parent.gameObject);
                    CombatController enemyCombat = gameObjects[i].transform.GetComponent<CombatController>();
                    myStats = gameObjects[i].transform.GetComponent<CharacterStats>();

                    if (enemyCombat != null)
                    {
                        Debug.Log(enemyCombat + " " + myStats);

                        this.GetComponent<CombatController>().Attack(myStats);

                        try
                        {
                            if (gameObjects[i] != null)
                            {
                                if (gameObjects[i].GetComponent<EnemyMelee>() != null)
                                {
                                    gameObjects[i].transform.GetComponent<EnemyMelee>().healthBar.SetActive(true);
                                    gameObjects[i].transform.GetComponent<EnemyMelee>().hpTimer = 2;

                                    EnemyMelee enemyScript = gameObjects[i].GetComponent<EnemyMelee>();

                                    enemyScript.knockbackTimer = 1;
                                }
                                if (gameObjects[i].GetComponent<EnemyRanged>() != null)
                                {
                                    gameObjects[i].transform.GetComponent<EnemyRanged>().healthBar.SetActive(true);
                                    gameObjects[i].transform.GetComponent<EnemyRanged>().hpTimer = 2;
                                }

                                gameObjects[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObjects[i].GetComponent<Rigidbody2D>().velocity.y);

                                if (GetComponent<Player>().flipped)
                                {
                                    gameObjects[i].transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(-knockback, knockback));
                                }
                                else
                                {
                                    gameObjects[i].transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, knockback));
                                }
                            }
                        }
                        catch
                        {

                        }
                    }

                    FindObjectOfType<AudioManager>().Play("Hit Melee");
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
    }

    public void Shield()
    {
        playerStats.defence.AddModifier(shieldDefence);
    }

    public void StopBlocking()
    {
        playerStats.defence.RemoveModifier(shieldDefence);
    }

    public IEnumerator MeleeAttack(float time) {
        yield return new WaitForSeconds(time);
        delayfinished = false;
    }
}
