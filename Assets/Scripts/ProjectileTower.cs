using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    private DefencePeople theTower;

    public GameObject projectile;
    public Transform firePoint;

    private Transform target;
    public Transform launcherModel;

    public GameObject shotEffect;

    private Animator animator; // добавлено для анимации
    private static readonly int isShooting = Animator.StringToHash("IsShooting"); // хэш параметра анимации

    public float animationSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        theTower = GetComponent<DefencePeople>();
        animator = GetComponent<Animator>(); // инициализация аниматора
        animator.speed = animationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
       
        // Проверяем наличие цели (врага)
        if (target != null)
        {
            animator.SetBool(isShooting, true);
            //launcherModel.LookAt(target);
            launcherModel.rotation = Quaternion.Slerp(launcherModel.rotation, Quaternion.LookRotation(target.position - transform.position), 5f * Time.deltaTime);

            launcherModel.rotation = Quaternion.Euler(0f, launcherModel.rotation.eulerAngles.y, 0f);
            firePoint.LookAt(target);


        }
        else
        {
            
                animator.SetBool(isShooting, false);
            
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

    
    public void FireProjectile()
    {
       
        Instantiate(projectile, firePoint.position, firePoint.rotation);
        Instantiate(shotEffect, firePoint.position, firePoint.rotation);
    }

   
}
