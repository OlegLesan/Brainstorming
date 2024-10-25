using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        instance = this;
    }

    public bool levelActive;
    private bool levelVictory;

    private Base theBase;

    public List<EnemyHealthController> activeEnemies = new List<EnemyHealthController>();

    private SimpleEnemySpawner enemySpawner;

    void Start()
    {
        theBase  = FindObjectOfType<Base>();
        enemySpawner = FindObjectOfType<SimpleEnemySpawner>();
                    }

    void Update()
    {
        if(levelActive)
        {
            if(theBase.currentHealth <= 0)
            {
                levelActive = false;
                levelVictory = false;

                Debug.Log("Level Failed");
            }
            if(activeEnemies.Count == 0 && enemySpawner.amountToSpawn == 0)
            {
                levelActive = false;
                levelVictory = true;

                Debug.Log("Level Won");
            }
        }
    }
}
