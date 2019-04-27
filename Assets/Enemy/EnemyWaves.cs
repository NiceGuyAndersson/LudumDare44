﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    public EnemyWave enemyWavePrefab;

    public GameObject enemyTypeOne; // TODO Change name?
    public GameObject enemyTypeTwo;
    public GameObject enemyTypeThree;

    int enemyWaveCounter = 1;
    EnemyWave currentEnemyWave;
    PlayerController playerController;
    EnemySpawnerController enemySpawnerController;
    bool forcedNextWave = false;

    public void Reset()
    {
        enemyWaveCounter = 1;
        Destroy(currentEnemyWave);
    }

    private void Start()
    {
        playerController = Utilities.Scene.findExactlyOne<PlayerController>();
        enemySpawnerController = Utilities.Scene.findExactlyOne<EnemySpawnerController>();
    }

    private bool TimeForNextWave()
    {
        if (forcedNextWave)
        {
            forcedNextWave = false;
            return true;
        }
        return false; // TODO always false, fix later

        if(currentEnemyWave != null && currentEnemyWave.GetAliveEnemies() == 0)
        {
            return true;
        }
        return false;
    }

    void FixedUpdate()
    {
        if(TimeForNextWave())
        {
            NextWave();
        }
    }

    public void ForceNextWave()
    {
        forcedNextWave = true;
    }

    void NextWave()
    {
        currentEnemyWave = BuildNextWave(enemyWaveCounter);
        ++enemyWaveCounter;
    }

    // TODO This function can be moved out of this class.
    private EnemyWave BuildNextWave(int enemyWaveCounter)
    {
        EnemyWave enemyWave = Instantiate(enemyWavePrefab);
        // TODO fins spawn position for enememies
        if(enemyWaveCounter == 1)
        {
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, 3, enemySpawnerController.GetEnemmySpawner(0)));
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, 3, enemySpawnerController.GetEnemmySpawner(1)));
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, 3, enemySpawnerController.GetEnemmySpawner(2)));
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, 3, enemySpawnerController.GetEnemmySpawner(3)));
        }
        else if(enemyWaveCounter == 2)
        {
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, 3, enemySpawnerController.GetEnemmySpawner(0)));
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, 3, enemySpawnerController.GetEnemmySpawner(1)));
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, 3, enemySpawnerController.GetEnemmySpawner(2)));
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, 3, enemySpawnerController.GetEnemmySpawner(3)));
        }
        else
        {
            // TODO This is just examples
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, 15 * enemyWaveCounter/3, enemySpawnerController.GetEnemmySpawner(3)));
        }
        return enemyWave;
    }

    private List<GameObject> BuildEnemyList(GameObject enemyPrefab, int amount, EnemySpawner enemySpawner)
    {
        List<GameObject> enemyList = new List<GameObject>();
        for(int i = 0; i < amount; ++i)
        {
            enemyList.Add(enemySpawner.QueueEnemy(enemyPrefab, enemySpawner));
        }
        return enemyList;
    }
}
