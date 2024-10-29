using System.Collections;
using System.Collections.Generic;
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

    public void PauseUnpause()
    {

    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelectScene);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}

