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
    public float borderThickness = 10f; // Ширина границы для движения мыши

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

        // Получаем ввод от клавиш WASD
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Отключаем движение по оси Y для горизонтального перемещения
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Движение от клавиш WASD
        move = (forward * v + right * h) * moveSpeed * Time.deltaTime;

        // Получаем текущие координаты мыши
        Vector3 mousePosition = Input.mousePosition;

        // Получаем размеры экрана
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Добавляем движение от границ экрана
        if (mousePosition.x <= borderThickness) // Левый край
        {
            move += -transform.right * moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.x >= screenWidth - borderThickness) // Правый край
        {
            move += transform.right * moveSpeed * Time.deltaTime;
        }

        if (mousePosition.y <= borderThickness) // Нижний край
        {
            move += -transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.y >= screenHeight - borderThickness) // Верхний край
        {
            move += transform.forward * moveSpeed * Time.deltaTime;
        }

        // Движение по вертикали (ограничение по Y)
        float newYPosition = Mathf.SmoothDamp(transform.position.y, targetYPosition, ref currentYVelocity, heightSmoothTime);
        move.y = newYPosition - transform.position.y;

        // Двигаем камеру
        characterController.Move(move);

        // Поворот камеры с помощью Q и E
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
