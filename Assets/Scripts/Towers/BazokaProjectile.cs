using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazokaProjectile : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;

    public float damageAmount;

    public GameObject impactEffect;

    void Start()
    {
        // ������������� ��������� �������� ��� �������� ������� �����
        theRB.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ������� ������ ����� � ����� ������������
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);

        // �������� �������� ����� � ������ BazokaImpact
        BazokaImpact impactScript = impact.GetComponent<BazokaImpact>();
        if (impactScript != null)
        {
            impactScript.SetDamageAmount(damageAmount);
        }

        // ���������� ������ ����� �������� �������
        Destroy(gameObject);
    }

    void Update()
    {
        // ��������� ����� �� ������� � ���������� ������, ���� ��� ���������
        if ((transform.position.y <= -10) || (transform.position.y >= 10))
        {
            Destroy(gameObject);
        }
    }
}

