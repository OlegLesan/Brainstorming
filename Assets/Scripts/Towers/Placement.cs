using UnityEngine;

public class Placement : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (TowerManager.instance != null)
        {
            // Показываем панель с кнопками выбора башен
            UIController.instance.ShowTowerButtons();

            // Передаем текущий объект в TowerManager для последующего удаления после установки башни
            TowerManager.instance.SetPlacementObject(this.gameObject);
        }
    }
}
