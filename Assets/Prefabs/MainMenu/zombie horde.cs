using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieHorde : MonoBehaviour
{
    private SoundPlayer soundPlayer;

    private void Start()
    {
        // Проверяем, что текущая сцена - Main Menu
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            soundPlayer = GetComponent<SoundPlayer>();
            if (soundPlayer != null)
            {
                soundPlayer.PlaySound(0);
            }
            else
            {
                Debug.LogWarning("SoundPlayer компонент не найден на объекте.");
            }
        }
    }

    

    
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Отключаем объект, если текущая сцена не "Main Menu"
            gameObject.SetActive(scene.name == "Main Menu");
        }

    
}
