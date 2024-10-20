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
    public float borderThickness = 10f; // Это поле можно удалить, если оно больше не нужно

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

        // Получаем значения для осей горизонтального и вертикального движения
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Определяем направления вперед и вправо с нормализованными векторами
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Рассчитываем движение по осям с учетом скорости перемещения
        move = (forward * v + right * h) * moveSpeed * Time.deltaTime;

        // Контролируем вертикальное положение камеры с использованием SmoothDamp
        float newYPosition = Mathf.SmoothDamp(transform.position.y, targetYPosition, ref currentYVelocity, heightSmoothTime);
        move.y = newYPosition - transform.position.y;

        // Двигаем камеру
        characterController.Move(move);

        // Управляем поворотом камеры с помощью клавиш Q и E
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
