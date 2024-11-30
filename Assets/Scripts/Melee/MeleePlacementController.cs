using UnityEngine;
using UnityEngine.UI;

public class MeleePlacementController : MonoBehaviour
{
    public GameObject meleePrefab; // Префаб, который будет создан
    public LayerMask groundLayer;  // Слой, на который можно размещать
    public Button meleeButton;     // Кнопка ближнего боя

    private GameObject previewObject; // Объект для предварительного отображения
    private bool isPlacingMelee = false; // Режим размещения

    void Start()
    {
        // Настраиваем кнопку
        if (meleeButton != null)
        {
            meleeButton.onClick.AddListener(StartPlacingMelee);
            Debug.Log("Кнопка ближнего боя настроена");
        }

        // Создаем объект-превью, но скрываем его
        if (meleePrefab != null)
        {
            previewObject = Instantiate(meleePrefab);
            previewObject.GetComponent<Collider>().enabled = false; // Отключаем коллайдер
            previewObject.layer = LayerMask.NameToLayer("Ignore Raycast"); // Игнорируем Raycast
            previewObject.SetActive(false); // Скрываем по умолчанию
            Debug.Log("Объект-превью создан и скрыт по умолчанию");
        }
        else
        {
            Debug.LogWarning("MeleePrefab не установлен!");
        }
    }

    void Update()
    {
        if (isPlacingMelee)
        {
            HandlePreviewPosition(); // Управляем позицией объекта-превью
        }
    }

    // Начало процесса размещения
    private void StartPlacingMelee()
    {
        if (previewObject == null)
        {
            Debug.LogWarning("Превью объект не установлен!");
            return;
        }

        isPlacingMelee = true;
        meleeButton.gameObject.SetActive(false); // Скрываем кнопку ближнего боя
        previewObject.SetActive(true); // Показываем объект-превью
        Debug.Log("Начат процесс размещения, кнопка скрыта");
    }

    // Обработка позиции объекта-превью
    private void HandlePreviewPosition()
    {
        // Отправляем луч из камеры в позицию курсора
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Проверяем попадание на слой
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // Если курсор на слое, обновляем позицию объекта-превью
            previewObject.transform.position = hit.point;
            previewObject.SetActive(true); // Убеждаемся, что превью видно
            
        }
        else
        {
            
            previewObject.SetActive(false);
        }

        // Обработка кликов мыши
        HandleMouseClicks();
    }

    // Обработка кликов мыши
    private void HandleMouseClicks()
    {
        // Левый клик: создаем объект
        if (Input.GetMouseButtonDown(0) && previewObject.activeSelf)
        {
            PlaceMeleeObject(previewObject.transform.position);
        }

        // Правый клик: отменяем размещение
        if (Input.GetMouseButtonDown(1))
        {
            CancelMeleePlacement();
        }
    }

    // Установка объекта на слой
    private void PlaceMeleeObject(Vector3 position)
    {
        Instantiate(meleePrefab, position, Quaternion.identity); // Создаем финальный объект
        
        EndPlacingMelee();
    }

    // Отмена процесса размещения
    private void CancelMeleePlacement()
    {
        previewObject.SetActive(false); // Скрываем объект-превью
        
        EndPlacingMelee();
    }

    // Завершение процесса размещения
    private void EndPlacingMelee()
    {
        isPlacingMelee = false;
        meleeButton.gameObject.SetActive(true); // Показываем кнопку ближнего боя
        
    }
}
