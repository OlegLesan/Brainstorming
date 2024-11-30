using UnityEngine;
using UnityEngine.UI;

public class MeleePlacementController : MonoBehaviour
{
    public GameObject meleePrefab; // ������, ������� ����� ������
    public LayerMask groundLayer;  // ����, �� ������� ����� ���������
    public Button meleeButton;     // ������ �������� ���

    private GameObject previewObject; // ������ ��� ���������������� �����������
    private bool isPlacingMelee = false; // ����� ����������

    void Start()
    {
        // ����������� ������
        if (meleeButton != null)
        {
            meleeButton.onClick.AddListener(StartPlacingMelee);
            Debug.Log("������ �������� ��� ���������");
        }

        // ������� ������-������, �� �������� ���
        if (meleePrefab != null)
        {
            previewObject = Instantiate(meleePrefab);
            previewObject.GetComponent<Collider>().enabled = false; // ��������� ���������
            previewObject.layer = LayerMask.NameToLayer("Ignore Raycast"); // ���������� Raycast
            previewObject.SetActive(false); // �������� �� ���������
            Debug.Log("������-������ ������ � ����� �� ���������");
        }
        else
        {
            Debug.LogWarning("MeleePrefab �� ����������!");
        }
    }

    void Update()
    {
        if (isPlacingMelee)
        {
            HandlePreviewPosition(); // ��������� �������� �������-������
        }
    }

    // ������ �������� ����������
    private void StartPlacingMelee()
    {
        if (previewObject == null)
        {
            Debug.LogWarning("������ ������ �� ����������!");
            return;
        }

        isPlacingMelee = true;
        meleeButton.gameObject.SetActive(false); // �������� ������ �������� ���
        previewObject.SetActive(true); // ���������� ������-������
        Debug.Log("����� ������� ����������, ������ ������");
    }

    // ��������� ������� �������-������
    private void HandlePreviewPosition()
    {
        // ���������� ��� �� ������ � ������� �������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ��������� ��������� �� ����
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // ���� ������ �� ����, ��������� ������� �������-������
            previewObject.transform.position = hit.point;
            previewObject.SetActive(true); // ����������, ��� ������ �����
            
        }
        else
        {
            
            previewObject.SetActive(false);
        }

        // ��������� ������ ����
        HandleMouseClicks();
    }

    // ��������� ������ ����
    private void HandleMouseClicks()
    {
        // ����� ����: ������� ������
        if (Input.GetMouseButtonDown(0) && previewObject.activeSelf)
        {
            PlaceMeleeObject(previewObject.transform.position);
        }

        // ������ ����: �������� ����������
        if (Input.GetMouseButtonDown(1))
        {
            CancelMeleePlacement();
        }
    }

    // ��������� ������� �� ����
    private void PlaceMeleeObject(Vector3 position)
    {
        Instantiate(meleePrefab, position, Quaternion.identity); // ������� ��������� ������
        
        EndPlacingMelee();
    }

    // ������ �������� ����������
    private void CancelMeleePlacement()
    {
        previewObject.SetActive(false); // �������� ������-������
        
        EndPlacingMelee();
    }

    // ���������� �������� ����������
    private void EndPlacingMelee()
    {
        isPlacingMelee = false;
        meleeButton.gameObject.SetActive(true); // ���������� ������ �������� ���
        
    }
}
