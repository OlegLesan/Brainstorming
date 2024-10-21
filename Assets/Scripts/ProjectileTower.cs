using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    private DefencePeople theTower;

    public GameObject projectile;
    public Transform firePoint;
    public float timeBetweenShots = 1f;
    private float shotCounter;

    private Transform target;
    public Transform launcherModel;

    void Start()
    {
        theTower = GetComponent<DefencePeople>();
    }

    void Update()
    {
        if (target != null)
        {
            // ѕлавный поворот модели башни к цели
            Vector3 direction = target.position - launcherModel.position;
            launcherModel.rotation = Quaternion.Slerp(
                launcherModel.rotation,
                Quaternion.LookRotation(direction, Vector3.up), // ”казываем "вверх" как Vector3.up
                5f * Time.deltaTime  // —корость поворота
            );
            launcherModel.rotation = Quaternion.Euler(0f, launcherModel.rotation.eulerAngles.y, 0f);
        }
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0 && target != null)
        {
            shotCounter = timeBetweenShots;

            firePoint.LookAt(target);

            Instantiate(projectile, firePoint.position, firePoint.rotation);

        }
        if (theTower.enemiesInRange.Count > 0)
        {
            float minDistance = theTower.range + 1f;
            foreach (EnemyControler enemy in theTower.enemiesInRange)
            {
                if (enemy != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = enemy.transform;
                    }
                }
            }
        }
        else
        {
            target = null;
        }
    }
}