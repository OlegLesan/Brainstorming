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
    public float rotationStep = 90f; // ��� �������� (90 ��������)
    public float borderThickness = 10f; // ��� ���� ����� �������, ���� ��� ������ �� �����

    private CharacterController characterController;
    private float targetYPosition;
    private float currentYVelocity;
    private bool isRotating = false;
    private Quaternion targetRotation;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        targetYPosition = transform.position.y;
        targetRotation = transform.rotation; // ���������� ������� ������� ����� ��������
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

        // ��������� ������� ������ Q ��� E ��� �������� �� 90 ��������
        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // ������������ �� -90 ��������
                targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - rotationStep, transform.eulerAngles.z);
                isRotating = true;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                // ������������ �� 90 ��������
                targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + rotationStep, transform.eulerAngles.z);
                isRotating = true;
            }
        }

        // ���� ������� ������� ��������, ������ ������� ������
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            // ���������, �������� �� �� �������� ��������
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }
}
