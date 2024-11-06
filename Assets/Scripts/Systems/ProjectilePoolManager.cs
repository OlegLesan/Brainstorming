using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolManager : MonoBehaviour
{
    public static ProjectilePoolManager instance;

    [System.Serializable]
    public class ProjectilePool
    {
        public string projectileTag; // ���������� ��� ��� ������� (��������, "Bazoka", "Arrow")
        public GameObject projectilePrefab; // ������ �������
        public int poolSize; // ������ ����
        public Queue<GameObject> poolQueue; // ������� �������� ��� ����

        public void InitializePool()
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

    public List<ProjectilePool> projectilePools; // ������ ����� ��� ������ ����� ��������

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        InitializePools();
    }

    private void InitializePools()
    {
        foreach (ProjectilePool pool in projectilePools)
        {
            pool.InitializePool();
        }
    }

    public GameObject GetProjectile(string tag)
    {
        ProjectilePool pool = projectilePools.Find(p => p.projectileTag == tag);

        if (pool != null)
        {
            return pool.GetProjectile();
        }

        Debug.LogWarning("No pool found with tag: " + tag);
        return null;
    }

    public void ReturnProjectile(string tag, GameObject projectile)
    {
        ProjectilePool pool = projectilePools.Find(p => p.projectileTag == tag);

        if (pool != null)
        {
            pool.ReturnProjectile(projectile);
        }
        else
        {
            Destroy(projectile);
        }
    }
}
