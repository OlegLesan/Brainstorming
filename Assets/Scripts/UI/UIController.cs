using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    public string levelSelectScene, mainMenuScene;
    public GameObject pauseScreen;

    void Start()
    {
        // ��������, ��� ������ ������ ����� ���������� ���������
        HideTowerButtons();
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

    // ���������� ������ � �������� ������ �����
    public void ShowTowerButtons()
    {
        towerButtons.SetActive(true);
    }

    // �������� ������ � �������� ����� ������ �����
    public void HideTowerButtons()
    {
        towerButtons.SetActive(false);
    }
}
