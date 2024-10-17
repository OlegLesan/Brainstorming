using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Cameraсontroller : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float verticalSpeed = 5f;
    public float rotateSpeed = 90f;
    public float minYPosition = 23f;
    public float maxYPosition = 76f;
    public float heightSmoothTime = 0.1f; // Время сглаживания высоты

    private CharacterController characterController;
    private float targetYPosition;
    private float currentYVelocity; // Используется для SmoothDamp

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        targetYPosition = transform.position.y; // Инициализируем начальную высоту
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Перемещение камеры
        Vector3 move = (forward * v + right * h) * moveSpeed * Time.deltaTime;

        // Изменение высоты камеры
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            targetYPosition += scroll * verticalSpeed * Time.deltaTime;
            targetYPosition = Mathf.Clamp(targetYPosition, minYPosition, maxYPosition);
        }

        // Плавная интерполяция высоты
        float newYPosition = Mathf.SmoothDamp(transform.position.y, targetYPosition, ref currentYVelocity, heightSmoothTime);
        move.y = newYPosition - transform.position.y; // Устанавливаем изменение высоты в вектор перемещения

        // Перемещение с помощью CharacterController
        characterController.Move(move);

        // Поворот камеры
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
