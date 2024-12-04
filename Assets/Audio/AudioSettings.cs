using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    private static bool isMuted; // Флаг состояния звука (сохраняется между сценами)
    private static bool initialized = false; // Флаг для проверки инициализации

    [SerializeField] private Image buttonImage; // Ссылка на компонент Image кнопки
    [SerializeField] private Sprite soundOnSprite; // Спрайт для включённого звука
    [SerializeField] private Sprite soundOffSprite; // Спрайт для выключенного звука

    private void Awake()
    {
        // Проверяем, была ли настройка звука уже загружена
        if (!initialized)
        {
            // Загружаем состояние звука
            isMuted = PlayerPrefs.GetInt("AudioMuted", 0) == 1;
            AudioListener.pause = isMuted;
            initialized = true;
        }

        // Обновляем спрайт кнопки при старте
        UpdateButtonSprite();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted; // Переключаем флаг
        AudioListener.pause = isMuted; // Ставим звук на паузу или включаем

        // Сохраняем состояние звука
        PlayerPrefs.SetInt("AudioMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // Обновляем спрайт кнопки
        UpdateButtonSprite();
    }

    private void UpdateButtonSprite()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }
}
