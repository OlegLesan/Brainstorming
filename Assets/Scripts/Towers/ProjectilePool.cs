using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int poolSize = 10;
    private Queue<GameObject> poolQueue;

    void Awake()
    {
        poolQueue = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            poolQueue.Enqueue(projectile);
        }
    }

    public GameObject GetProjectile()
    {
        if (poolQueue.Count > 0)
        {
            GameObject projectile = poolQueue.Dequeue();
            projectile.SetActive(true);
            return projectile;
        }
        else
        {
            GameObject newProjectile = Instantiate(projectilePrefab);
            return newProjectile;
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        poolQueue.Enqueue(projectile);
    }
}
