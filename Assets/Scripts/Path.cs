using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Transform[] points;

    // Этот метод возвращает случайную дочернюю точку из указанного родительского Transform.
    public Transform GetRandomChildPoint(Transform parent)
    {
        if (parent.childCount == 0)
            return parent; // Если нет дочерних объектов, возвращаем сам объект.

        int randomIndex = Random.Range(0, parent.childCount);
        return parent.GetChild(randomIndex);
    }
}
