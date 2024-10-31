using UnityEngine;

public class Placement : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (TowerManager.instance != null)
        {
            // ���������� ������ � �������� ������ �����
            UIController.instance.ShowTowerButtons();

            // �������� ������� ������ � TowerManager ��� ������������ �������� ����� ��������� �����
            TowerManager.instance.SetPlacementObject(this.gameObject);
        }
    }
}
