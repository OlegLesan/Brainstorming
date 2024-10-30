using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public string newGameScene;
    public string levelSelect;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMenuMusic();
    }

    

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);

    }
    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
