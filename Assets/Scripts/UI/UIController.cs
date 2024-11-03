using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text goldText;
    public GameObject notEnoughMoneyWarning;
    public GameObject levelCompleteScreen, levelFailScreen;
    public GameObject towerButtons;
    public GameObject upgradePanel; // Панель улучшений
    public Button upgradeButton; // Кнопка улучшения
    public TMP_Text upgradeCostText; // Текст для отображения стоимости улучшения

    public string levelSelectScene, mainMenuScene;
    public GameObject pauseScreen;

    void Start()
    {
        HideTowerButtons();
        HideUpgradePanel(); // Скрыть панель улучшений по умолчанию
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
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

    // Показать панель улучшений
    public void ShowUpgradePanel(Tower tower)
    {
        upgradePanel.SetActive(true);
        upgradeCostText.text = $"{tower.upgradeCost}G"; // Отображаем стоимость улучшения

        // Проверяем, хватает ли денег на улучшение
        bool canAffordUpgrade = MoneyManager.instance.currentMoney >= tower.upgradeCost;
        upgradeButton.interactable = canAffordUpgrade; // Включаем или отключаем кнопку
    }

    // Скрыть панель улучшений
    public void HideUpgradePanel()
    {
        upgradePanel.SetActive(false);
    }

    // Улучшить выбранную башню
    public void UpgradeSelectedTower()
    {
        if (TowerManager.instance.selectedTower != null)
        {
            TowerManager.instance.selectedTower.UpgradeTower(); // Улучшить башню
            HideUpgradePanel(); // Скрыть панель после улучшения
        }
    }
}
