using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cameracontroller : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 90f;
    public float minYPosition = 23f;
    public float maxYPosition = 76f;
    public float heightSmoothTime = 0.1f;
    public float borderThickness = 10f; // ��� ���� ����� �������, ���� ��� ������ �� �����

    private CharacterController characterController;
    private float targetYPosition;
    private float currentYVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        targetYPosition = transform.position.y;
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        // �������� �������� ��� ���� ��������������� � ������������� ��������
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // ���������� ����������� ������ � ������ � ���������������� ���������
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // ������������ �������� �� ���� � ������ �������� �����������
        move = (forward * v + right * h) * moveSpeed * Time.deltaTime;

        // ������������ ������������ ��������� ������ � �������������� SmoothDamp
        float newYPosition = Mathf.SmoothDamp(transform.position.y, targetYPosition, ref currentYVelocity, heightSmoothTime);
        move.y = newYPosition - transform.position.y;

        // ������� ������
        characterController.Move(move);

        // ��������� ��������� ������ � ������� ������ Q � E
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}
