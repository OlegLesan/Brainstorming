using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Cameracontroller : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 90f;
    public float minYPosition = 23f;
    public float maxYPosition = 76f;
    public float heightSmoothTime = 0.1f;
    public float borderThickness = 10f; // ������ ������� ��� �������� ����

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

        // �������� ���� �� ������ WASD
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // ��������� �������� �� ��� Y ��� ��������������� �����������
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // �������� �� ������ WASD
        move = (forward * v + right * h) * moveSpeed * Time.deltaTime;

        // �������� ������� ���������� ����
        Vector3 mousePosition = Input.mousePosition;

        // �������� ������� ������
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // ��������� �������� �� ������ ������
        if (mousePosition.x <= borderThickness) // ����� ����
        {
            move += -transform.right * moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.x >= screenWidth - borderThickness) // ������ ����
        {
            move += transform.right * moveSpeed * Time.deltaTime;
        }

        if (mousePosition.y <= borderThickness) // ������ ����
        {
            move += -transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.y >= screenHeight - borderThickness) // ������� ����
        {
            move += transform.forward * moveSpeed * Time.deltaTime;
        }

        // �������� �� ��������� (����������� �� Y)
        float newYPosition = Mathf.SmoothDamp(transform.position.y, targetYPosition, ref currentYVelocity, heightSmoothTime);
        move.y = newYPosition - transform.position.y;

        // ������� ������
        characterController.Move(move);

        // ������� ������ � ������� Q � E
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
