using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public EnemyControler enemyToSpawn;
    public Transform spawnPoint;

    public float timeBetweenSpawns;
    private float spawnCounter;
    public int amountToSpawn = 15;
    public Path thePath;

    void Start()
    {
        spawnCounter = timeBetweenSpawns;
    }


    void Update()
    {
        if (amountToSpawn > 0 && LevelManager.instance.levelActive)
        {
            spawnCounter -= Time.deltaTime;
            if (spawnCounter <= 0)
            {
                spawnCounter = timeBetweenSpawns;

                Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation).Setup(thePath);
                amountToSpawn--;
            }
        }
    }
}
