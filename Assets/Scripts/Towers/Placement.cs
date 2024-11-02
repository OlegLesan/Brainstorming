using UnityEngine;

public class Placement : MonoBehaviour
{
    public GameObject arrow; // Стрелка, которая будет включаться и вращаться
    public GameObject fx; // FX, который будет отключаться
    public float rotationSpeed = 50f; // Скорость вращения стрелки

    private static Placement currentActivePlacement; // Хранит текущий активный объект

    private void OnMouseDown()
    {
        // Если уже есть активный объект, скрываем его стрелку и включаем его FX
        if (currentActivePlacement != null && currentActivePlacement != this)
        {
            currentActivePlacement.HideArrowAndShowFX();
        }

        // Устанавливаем текущий объект как активный
        currentActivePlacement = this;

        // Включаем стрелку и отключаем FX для текущего объекта
        ShowArrowAndHideFX();

        // Показать кнопки башни
        UIController.instance.ShowTowerButtons();

        // Установить объект размещения в TowerManager
        TowerManager.instance.SetPlacementObject(this.gameObject);
    }

    private void Update()
    {
        // Если стрелка активна, вращаем ее вокруг оси Y
        if (arrow != null && arrow.activeSelf)
        {
            // Вращаем только по оси Y
            arrow.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }
    }

    public void ShowArrowAndHideFX()
    {
        if (arrow != null)
            arrow.SetActive(true);

        if (fx != null)
            fx.SetActive(false);
    }

    public void HideArrowAndShowFX()
    {
        if (arrow != null)
            arrow.SetActive(false);

        if (fx != null)
            fx.SetActive(true);
    }
}
