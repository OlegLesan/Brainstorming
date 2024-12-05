using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieHorde : MonoBehaviour
{
    private SoundPlayer soundPlayer;

    private void Start()
    {
        // ���������, ��� ������� ����� - Main Menu
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            soundPlayer = GetComponent<SoundPlayer>();
            if (soundPlayer != null)
            {
                soundPlayer.PlaySound(0);
            }
            else
            {
                Debug.LogWarning("SoundPlayer ��������� �� ������ �� �������.");
            }
        }
    }

    

    
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // ��������� ������, ���� ������� ����� �� "Main Menu"
            gameObject.SetActive(scene.name == "Main Menu");
        }

    
}
