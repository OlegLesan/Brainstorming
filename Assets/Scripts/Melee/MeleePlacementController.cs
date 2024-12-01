using UnityEngine;
using UnityEngine.UI;
using TMPro; // Для работы с TMP_Text
using System.Collections;
using System.Collections.Generic;

public class MeleePlacementController : MonoBehaviour
{
    public GameObject meleePrefab; // Префаб юнита
    public LayerMask groundLayer; // Слой, на который можно размещать
    public Button meleeButton; // Кнопка ближнего боя
    public int initialPoolSize = 10; // Начальный размер пула
    public int meleeUnitCost = 50; // Стоимость юнита
    public TMP_Text meleeCostText; // Текст для отображения стоимости юнита

    private Queue<GameObject> unitPool; // Пул объектов
    private GameObject previewObject; // Объект-превью
    private bool isPlacingMelee = false; // Режим размещения

    void Start()
    {
        InitializePool();

        if (meleeButton != null)
        {
            meleeButton.onClick.AddListener(StartPlacingMelee);
        }

        if (meleePrefab != null)
        {
            previewObject = Instantiate(meleePrefab);
            previewObject.GetComponent<Collider>().enabled = false;
            previewObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            previewObject.SetActive(false);
        }

        // Устанавливаем текст цены юнита
        if (meleeCostText != null)
        {
            meleeCostText.text = $"{meleeUnitCost}G";
        }
    }

    void Update()
    {
        if (isPlacingMelee)
        {
            HandlePreviewPosition();
        }
    }

    private void InitializePool()
    {
        unitPool = new Queue<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject unit = Instantiate(meleePrefab);
            unit.SetActive(false);
            unitPool.Enqueue(unit);
        }
    }

    public GameObject GetUnitFromPool()
    {
        if (unitPool.Count > 0)
        {
            GameObject unit = unitPool.Dequeue();
            unit.SetActive(true);
            unit.GetComponent<MeleeUnitController>().enabled = true; // Активируем логику юнита
            return unit;
        }
        else
        {
            Debug.LogWarning("Пул пуст. Создаётся новый юнит.");
            return Instantiate(meleePrefab);
        }
    }

    public void ReturnUnitToPool(GameObject unit)
    {
        unit.SetActive(false);
        unit.GetComponent<MeleeUnitController>().enabled = false; // Отключаем логику юнита
        unitPool.Enqueue(unit);
    }

    private void StartPlacingMelee()
    {
        if (previewObject == null) return;

        isPlacingMelee = true;
        meleeButton.gameObject.SetActive(false);
        previewObject.SetActive(true);
    }

    private void HandlePreviewPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            previewObject.transform.position = hit.point;
            previewObject.SetActive(true);
        }
        else
        {
            previewObject.SetActive(false);
        }

        HandleMouseClicks();
    }

    private void HandleMouseClicks()
    {
        if (Input.GetMouseButtonDown(0) && previewObject.activeSelf)
        {
            PlaceMeleeObject(previewObject.transform.position);
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelMeleePlacement();
        }
    }

    private void PlaceMeleeObject(Vector3 position)
    {
        if (!MoneyManager.instance.SpendMoney(meleeUnitCost))
        {
            if (UIController.instance.notEnoughMoneyWarning != null)
            {
                UIController.instance.notEnoughMoneyWarning.SetActive(true);
                StartCoroutine(HideNotEnoughMoneyWarning());
            }
            return; // Выходим, если недостаточно денег
        }

        GameObject unit = GetUnitFromPool();
        unit.transform.position = position;
        unit.transform.rotation = Quaternion.identity;

        if (previewObject != null)
        {
            previewObject.SetActive(false);
        }

        EndPlacingMelee();
    }

    private IEnumerator HideNotEnoughMoneyWarning()
    {
        yield return new WaitForSeconds(2f);
        if (UIController.instance.notEnoughMoneyWarning != null)
        {
            UIController.instance.notEnoughMoneyWarning.SetActive(false);
        }
    }

    private void CancelMeleePlacement()
    {
        previewObject.SetActive(false);
        EndPlacingMelee();
    }

    private void EndPlacingMelee()
    {
        isPlacingMelee = false;
        meleeButton.gameObject.SetActive(true);
    }
}
