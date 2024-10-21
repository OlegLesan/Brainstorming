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
    public float rotationStep = 90f; // Ўаг поворота (90 градусов)
    public float borderThickness = 10f; // Ёто поле можно удалить, если оно больше не нужно

    private CharacterController characterController;
    private float targetYPosition;
    private float currentYVelocity;
    private bool isRotating = false;
    private Quaternion targetRotation;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        targetYPosition = transform.position.y;
        targetRotation = transform.rotation; // »значально целевой поворот равен текущему
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        // ѕолучаем значени€ дл€ осей горизонтального и вертикального движени€
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // ќпредел€ем направлени€ вперед и вправо с нормализованными векторами
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // –ассчитываем движение по ос€м с учетом скорости перемещени€
        move = (forward * v + right * h) * moveSpeed * Time.deltaTime;

        //  онтролируем вертикальное положение камеры с использованием SmoothDamp
        float newYPosition = Mathf.SmoothDamp(transform.position.y, targetYPosition, ref currentYVelocity, heightSmoothTime);
        move.y = newYPosition - transform.position.y;

        // ƒвигаем камеру
        characterController.Move(move);

        // ѕровер€ем нажатие клавиш Q или E дл€ поворота на 90 градусов
        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // ѕоворачиваем на -90 градусов
                targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - rotationStep, transform.eulerAngles.z);
                isRotating = true;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                // ѕоворачиваем на 90 градусов
                targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + rotationStep, transform.eulerAngles.z);
                isRotating = true;
            }
        }

        // ≈сли запущен процесс поворота, плавно вращаем камеру
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            // ѕровер€ем, достигли ли мы целевого поворота
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }
}
