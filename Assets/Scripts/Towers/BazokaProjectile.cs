using UnityEngine;

public class BazokaProjectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;
    public float damageAmount;
    public GameObject impactEffect;

    private ProjectileTower tower; // —сылка на башню, чтобы вернуть снар€д в пул

    void Start()
    {
        theRB.velocity = transform.forward * moveSpeed;
    }

    public void SetTower(ProjectileTower towerRef)
    {
        tower = towerRef;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
            BazokaImpact impactScript = impact.GetComponent<BazokaImpact>();
            if (impactScript != null)
            {
                impactScript.SetDamageAmount(damageAmount);
            }

            if (tower != null)
            {
                tower.ReturnProjectileToPool(gameObject);
            }
        }
    }

    void Update()
    {
        if ((transform.position.y <= -10) || (transform.position.y >= 10))
        {
            if (tower != null)
            {
                tower.ReturnProjectileToPool(gameObject);
            }
        }
    }
}
