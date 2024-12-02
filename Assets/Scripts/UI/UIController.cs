using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Texture2D customCursor; // Ваша текстура курсора
    public Vector2 cursorHotspot = Vector2.zero; // Точка привязки курсора

    public GameObject hotbar; // Панель выбора башен
    public TMP_Text goldText;
    public GameObject notEnoughMoneyWarning;
    public GameObject levelCompleteScreen, levelFailScreen;
    public GameObject towerButtons;
    public GameObject upgradePanel;
    public Button upgradeButton;
    public TMP_Text upgradeCostText;
    public Button sellButton;
    public TMP_Text sellValueText;

    public Button meleeButton; // Кнопка ближнего боя

    // Звуковые эффекты
    public AudioClip upgradeSound;
    public AudioClip sellSound;
    private AudioSource audioSource;

    public string levelSelectScene, mainMenuScene;
    public GameObject pauseScreen;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetCustomCursor(); // Устанавливаем кастомный курсор при запуске игры
        HideTowerButtons();
        HideUpgradePanel();
        HideHotbar(); // Скрываем hotbar по умолчанию
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void SetCustomCursor()
    {
        Cursor.SetCursor(customCursor, cursorHotspot, CursorMode.Auto);
    }

    public void ShowHotbar()
    {
        if (hotbar != null)
            hotbar.SetActive(true);
    }

    public void HideHotbar()
    {
        if (hotbar != null)
            hotbar.SetActive(false);
    }

    public void PauseUnpause()
    {
        if (levelCompleteScreen.activeSelf || levelFailScreen.activeSelf)
        {
            return;
        }

        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            AudioListener.volume = 0f; // Отключаем звук
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
            AudioListener.volume = 1f; // Включаем звук
        }
    }

    public void LevelSelect()
    {
        ResumeAudio();
        SceneManager.LoadScene(levelSelectScene);
    }

    public void MainMenu()
    {
        ResumeAudio();
        SceneManager.LoadScene(mainMenuScene);
    }

    public void TryAgain()
    {
        ResumeAudio();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        levelCompleteScreen.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.volume = 0f; // Отключаем звук
    }

    public void ShowLevelFailScreen()
    {
        levelFailScreen.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.volume = 0f; // Отключаем звук
    }

    public void ShowTowerButtons()
    {
        towerButtons.SetActive(true);
        SetMeleeButtonActive(false); // Скрываем кнопку ближнего боя
    }

    public void HideTowerButtons()
    {
        towerButtons.SetActive(false);
        SetMeleeButtonActive(true); // Показываем кнопку ближнего боя
    }

    public void ShowUpgradePanel(Tower tower)
    {
        upgradePanel.SetActive(true);
        upgradeCostText.text = $"{tower.upgradeCost}G";

        bool canUpgrade = tower.upgradedTowerPrefab != null && MoneyManager.instance.currentMoney >= tower.upgradeCost;
        upgradeButton.interactable = canUpgrade;

        sellButton.interactable = true;
        sellValueText.text = $"Sell: {tower.sellValue}G";

        SetMeleeButtonActive(false); // Скрываем кнопку ближнего боя
    }

    public void HideUpgradePanel()
    {
        upgradePanel.SetActive(false);
        SetMeleeButtonActive(true); // Показываем кнопку ближнего боя
    }

    public void UpgradeSelectedTower()
    {
        if (TowerManager.instance.selectedTower != null)
        {
            TowerManager.instance.selectedTower.UpgradeTower();
            PlaySound(upgradeSound);
            HideUpgradePanel();
        }
    }

    public void SellSelectedTower()
    {
        if (TowerManager.instance.selectedTower != null)
        {
            TowerManager.instance.selectedTower.SellTower();
            PlaySound(sellSound);
            HideUpgradePanel();
        }
    }

    private void SetMeleeButtonActive(bool isActive)
    {
        if (meleeButton != null)
        {
            meleeButton.gameObject.SetActive(isActive); // Управляем активностью объекта кнопки
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void ResumeAudio()
    {
        Time.timeScale = 1f;
        AudioListener.volume = 1f; // Включаем звук
    }
}
