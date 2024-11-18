using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroller : MonoBehaviour
{
    public float moveSpeed = 10f;            // Скорость движения
    public float rotateSpeed = 90f;         // Скорость вращения
    public float minYPosition = 23f;        // Минимальная высота
    public float maxYPosition = 76f;        // Максимальная высота
    public float minXPosition = -50f;       // Минимальная позиция по X
    public float maxXPosition = 50f;        // Максимальная позиция по X
    public float minZPosition = -50f;       // Минимальная позиция по Z
    public float maxZPosition = 50f;        // Максимальная позиция по Z
    public float smoothTime = 0.2f;         // Время сглаживания для движения
    public float rotationStep = 90f;        // Шаг поворота (90 градусов)
    public float scrollSpeed = 5f;          // Скорость изменения высоты колесиком мыши
    public float collisionRadius = 1f;      // Радиус для проверки столкновений
    public LayerMask collisionMask;         // Слой для проверки столкновений

    private Vector3 targetPosition;         // Целевая позиция камеры
    private float targetYPosition;          // Целевая высота
    private Quaternion targetRotation;      // Целевое вращение камеры
    private Vector3 velocity = Vector3.zero;// Скорость для сглаживания движения
    private bool isRotating = false;        // Флаг для проверки вращения

    void Start()
    {
        targetPosition = transform.position;
        targetYPosition = transform.position.y;
        targetRotation = transform.rotation; // Изначально целевое вращение равно текущему
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        SmoothMove();
    }

    void HandleMovement()
    {
        Vector3 move = Vector3.zero;

        // Получаем ввод пользователя
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Рассчитываем направления вперед и вправо
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Убираем компонент по оси Y для горизонтального движения
        forward.y = 0;
        right.y = 0;

        // Нормализуем направления
        forward.Normalize();
        right.Normalize();

        // Обновляем целевую позицию на основе ввода
        Vector3 desiredPosition = targetPosition + (forward * v + right * h) * moveSpeed * Time.unscaledDeltaTime;

        // Применяем ограничения по высоте
        targetYPosition += scroll * scrollSpeed;
        targetYPosition = Mathf.Clamp(targetYPosition, minYPosition, maxYPosition);
        desiredPosition.y = targetYPosition;

        // Применяем ограничения по X и Z
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minXPosition, maxXPosition);
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZPosition, maxZPosition);

        // Проверяем столкновение
        if (!Physics.CheckSphere(desiredPosition, collisionRadius, collisionMask))
        {
            targetPosition = desiredPosition; // Обновляем целевую позицию, если нет столкновений
        }
    }

    void HandleRotation()
    {
        // Проверяем ввод для вращения камеры
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

        // Плавное вращение
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.unscaledDeltaTime);

            // Проверяем, достигли ли мы целевого поворота
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    void SmoothMove()
    {
        // Плавное перемещение камеры
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
    }
}
