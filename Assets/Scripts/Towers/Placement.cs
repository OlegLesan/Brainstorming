using UnityEngine;
using UnityEngine.EventSystems;

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

        // Показать панель выбора башен (hotbar)
        UIController.instance.ShowHotbar();

        // Установить объект размещения в TowerManager
        TowerManager.instance.SetPlacementObject(this.gameObject);
    }

    private void Update()
    {
        // Если стрелка активна, вращаем её вокруг оси Y
        if (arrow != null && arrow.activeSelf)
        {
            // Вращаем только по оси Y
            arrow.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }

        // Проверка на клик в пустом месте
        if (Input.GetMouseButtonDown(0))
        {
            HandleClickOutside();
        }
    }

    private void HandleClickOutside()
    {
        // Проверяем, был ли клик по элементу UI, чтобы не скрывать hotbar
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Если клик по UI, не скрываем hotbar
        }

        // Определяем, был ли клик по объекту `Placement`
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Placement placement = hit.collider.GetComponent<Placement>();

            // Если клик был не по объекту `Placement`, скрываем стрелку и показываем FX у текущего активного объекта
            if (placement == null && currentActivePlacement != null)
            {
                currentActivePlacement.HideArrowAndShowFX();
                UIController.instance.HideHotbar(); // Скрыть панель выбора башен (hotbar)
                currentActivePlacement = null;
            }
        }
        else if (currentActivePlacement != null)
        {
            // Если клик вообще в пустое место, также скрываем стрелку и показываем FX у текущего активного объекта
            currentActivePlacement.HideArrowAndShowFX();
            UIController.instance.HideHotbar(); // Скрыть панель выбора башен (hotbar)
            currentActivePlacement = null;
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
