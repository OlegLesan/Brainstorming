using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemyPrefabs;  // ������ �������� ������ ��� ���� �����
    public int[] enemyCounts;  // ���������� ������� ���� ������ � ���� �����
    public float spawnInterval = 1f;  // �������� ����� �������� ������
}
