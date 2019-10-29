using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] spawners;
    public GameObject[] enemies;

    private float timer;
    public float resetTimer;

    private bool debugLevelActive;
    private bool waveOneSpawned;
    private bool waveTwoSpawned;
    private bool waveThreeSpawned;


    // Start is called before the first frame update
    void Start()
    {
        debugLevelActive = false;
        waveOneSpawned = false;
        waveTwoSpawned = false;
        waveThreeSpawned = false;

        timer = resetTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            debugLevelActive = true;
        }

        if (debugLevelActive)
        {
            StartDebugLevel();
            timer -= Time.deltaTime;
        }
    }

    void StartDebugLevel()
    {
        if (!waveOneSpawned)
        {
            WaveOne();
        }
        
        if (!waveTwoSpawned && waveOneSpawned && timer <= 0)
        {
            WaveTwo();
        }

        if (!waveThreeSpawned && waveTwoSpawned && timer <= 0)
        {
            WaveThree();
        }

        if (waveOneSpawned && waveTwoSpawned && waveThreeSpawned)
        {
            waveOneSpawned = false;
            waveTwoSpawned = false;
            waveThreeSpawned = false;
            debugLevelActive = false;
        }
    }

    private void WaveOne()
    {
        SpawnEnemy(0, 3);
        SpawnEnemy(1, 0);
        SpawnEnemy(1, 1);

        timer = resetTimer;
        waveOneSpawned = true;
    }

    private void WaveTwo()
    {
        SpawnEnemy(0, (int)Random.Range(0,2));
        SpawnEnemy(1, 2);
        SpawnEnemy(1, 3);

        timer = resetTimer;
        waveTwoSpawned = true;
    }

    private void WaveThree()
    {
        SpawnEnemy(0, 3);
        SpawnEnemy(0, 3);
        SpawnEnemy(0, 2);
        SpawnEnemy(0, 2);

        SpawnEnemy(1, 0);
        SpawnEnemy(1, 1);

        waveThreeSpawned = true;
    }

    private void SpawnEnemy(int enemyType, int spawnerPlace)
    {
        Instantiate(enemies[enemyType], spawners[spawnerPlace].transform.position, Quaternion.identity);
    }
}
