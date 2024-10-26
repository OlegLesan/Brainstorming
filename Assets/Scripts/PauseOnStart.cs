using UnityEngine;

public class PauseOnStart : MonoBehaviour
{
    void Start()
    {
        // ”станавливаем timeScale в 0, чтобы поставить игру на паузу
        Time.timeScale = 0f;
    }

    // ƒополнительный метод дл€ сн€ти€ игры с паузы
    public void ResumeGame()
    {
        Time.timeScale = 1f; // ¬осстанавливаем timeScale, чтобы сн€ть игру с паузы
    }
}
