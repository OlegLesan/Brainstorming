using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownTower : MonoBehaviour
{
    public Light spotlight; // —сылка на Spot Light фонарика
    public float slowDownAmount = 0.5f; //  оличество замедлени€
    public float checkInterval = 0.2f; // »нтервал проверки дл€ производительности

    private List<EnemyControler> affectedEnemies = new List<EnemyControler>();

    private void Start()
    {
        if (spotlight == null)
        {
            spotlight = GetComponent<Light>(); // јвтоматически находим компонент света, если он не назначен
        }

        // «апускаем проверку через определенные интервалы времени
        InvokeRepeating(nameof(CheckEnemiesInLightCone), 0f, checkInterval);
    }

    private void CheckEnemiesInLightCone()
    {
        // Ќаходим всех врагов в сцене
        EnemyControler[] allEnemies = FindObjectsOfType<EnemyControler>();

        // ѕроходим по всем врагам и провер€ем их рассто€ние и угол до фонарика
        foreach (EnemyControler enemy in allEnemies)
        {
            if (enemy != null)
            {
                Vector3 directionToEnemy = enemy.transform.position - spotlight.transform.position;
                float distanceToEnemy = directionToEnemy.magnitude;

                // ѕровер€ем, находитс€ ли враг в пределах радиуса света
                if (distanceToEnemy <= spotlight.range)
                {
                    // ѕровер€ем, находитс€ ли враг внутри угла освещени€ Spot Light
                    float angleToEnemy = Vector3.Angle(spotlight.transform.forward, directionToEnemy);

                    if (angleToEnemy <= spotlight.spotAngle / 2)
                    {
                        // ≈сли враг еще не замедлен другим фонариком
                        if (!affectedEnemies.Contains(enemy) && enemy.currentSlowingTower == null)
                        {
                            enemy.speedMod -= slowDownAmount;
                            enemy.currentSlowingTower = this;
                            affectedEnemies.Add(enemy);
                        }
                    }
                    else
                    {
                        // ¬раг вне конуса света, восстанавливаем скорость, если он был под этим фонариком
                        RemoveEnemyFromEffect(enemy);
                    }
                }
                else
                {
                    // ¬раг вне радиуса света, восстанавливаем скорость, если он был под этим фонариком
                    RemoveEnemyFromEffect(enemy);
                }
            }
        }
    }

    private void RemoveEnemyFromEffect(EnemyControler enemy)
    {
        if (affectedEnemies.Contains(enemy) && enemy.currentSlowingTower == this)
        {
            enemy.speedMod += slowDownAmount;
            enemy.currentSlowingTower = null;
            affectedEnemies.Remove(enemy);
        }
    }

    private void OnDisable()
    {
        // ¬осстанавливаем скорость дл€ всех врагов при отключении фонарика
        foreach (EnemyControler enemy in affectedEnemies)
        {
            if (enemy != null && enemy.currentSlowingTower == this)
            {
                enemy.speedMod += slowDownAmount;
                enemy.currentSlowingTower = null;
            }
        }
        affectedEnemies.Clear();
    }
}
