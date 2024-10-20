using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        theRB.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    


}
