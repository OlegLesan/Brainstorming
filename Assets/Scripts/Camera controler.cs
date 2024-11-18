using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroller : MonoBehaviour
{
    public float moveSpeed = 10f;            // �������� ��������
    public float rotateSpeed = 90f;         // �������� ��������
    public float minYPosition = 23f;        // ����������� ������
    public float maxYPosition = 76f;        // ������������ ������
    public float smoothTime = 0.2f;         // ����� ����������� ��� ��������
    public float rotationStep = 90f;        // ��� �������� (90 ��������)
    public float scrollSpeed = 5f;          // �������� ��������� ������ ��������� ����

    private Vector3 targetPosition;         // ������� ������� ������
    private float targetYPosition;          // ������� ������
    private Quaternion targetRotation;      // ������� �������� ������
    private Vector3 velocity = Vector3.zero;// �������� ��� ����������� ��������
    private bool isRotating = false;        // ���� ��� �������� ��������

    void Start()
    {
        targetPosition = transform.position;
        targetYPosition = transform.position.y;
        targetRotation = transform.rotation; // ���������� ������� �������� ����� ��������
    }

    void Update()
    {
        // ������ ������ ��������� �� ����������
        HandleMovement();
        HandleRotation();
        SmoothMove();
    }

    void HandleMovement()
    {
        Vector3 move = Vector3.zero;

        // �������� ���� ������������
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // ������������ ����������� ������ � ������
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // ������� ��������� �� ��� Y ��� ��������������� ��������
        forward.y = 0;
        right.y = 0;

        // ����������� �����������
        forward.Normalize();
        right.Normalize();

        // ��������� ������� ������� �� ������ �����
        targetPosition += (forward * v + right * h) * moveSpeed * Time.unscaledDeltaTime;

        // ��������� ������
        targetYPosition += scroll * scrollSpeed;
        targetYPosition = Mathf.Clamp(targetYPosition, minYPosition, maxYPosition);
        targetPosition.y = targetYPosition;
    }

    void HandleRotation()
    {
        // ��������� ���� ��� �������� ������
        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - rotationStep, transform.eulerAngles.z);
                isRotating = true;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + rotationStep, transform.eulerAngles.z);
                isRotating = true;
            }
        }

        // ������� ��������
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.unscaledDeltaTime);

            // ���������, �������� �� �� �������� ��������
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    void SmoothMove()
    {
        // ������� ����������� ������
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
    }
}
