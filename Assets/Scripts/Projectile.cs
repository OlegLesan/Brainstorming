using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;

    public float damageAmount;

    public GameObject impactEffect;

    

    void Start()
    {
        theRB.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" )
        {
            other.GetComponent<EnemyHealthController>().TakeDamage(damageAmount);
           
        }
        Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

    void Update()
    {

        if ((transform.position.y <= -10) || (transform.position.y >= 10))
        {
            Destroy(gameObject);  
        }
    }



}
