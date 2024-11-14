using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

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
        // Проверяем, включен ли экран завершения уровня
        if (levelCompleteScreen.activeSelf)
        {
            return; // Если включен, выходим из метода, чтобы не включать экран паузы
        }

        // Переключение паузы, если экран завершения уровня не активен
        if (pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void LevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelectScene);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(LevelManager.instance.nextLevel);
    }

    public void ShowLevelFailScreen()
    {
        levelFailScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowTowerButtons()
    {
        towerButtons.SetActive(true);
    }

    public void HideTowerButtons()
    {
        towerButtons.SetActive(false);
    }

    // Показать панель улучшений и обновить стоимость продажи
    public void ShowUpgradePanel(Tower tower)
    {
        upgradePanel.SetActive(true);
        upgradeCostText.text = $"{tower.upgradeCost}G"; // Отображаем стоимость улучшения

        bool canUpgrade = tower.upgradedTowerPrefab != null && MoneyManager.instance.currentMoney >= tower.upgradeCost;
        upgradeButton.interactable = canUpgrade; // Включаем или отключаем кнопку улучшения

        sellButton.interactable = true; // Включаем кнопку продажи
        sellValueText.text = $"Sell: {tower.sellValue}G"; // Отображаем стоимость продажи
    }

    public void HideUpgradePanel()
    {
        upgradePanel.SetActive(false);
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

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
