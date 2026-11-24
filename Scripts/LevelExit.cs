using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [Header("Progression")]
    [Tooltip("Укажи номер уровня, который откроется после этого портала (2 на Level1, 3 на Level2)")]
    [SerializeField] private int nextLevelToUnlock = 2;

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            isTriggered = true;

            AudioManager.Instance?.PlayWin();

            if (SaveManager.Instance != null)
            {
                int currentCoins = ScoreManager.instance != null ? ScoreManager.instance.coins : 0;
                SaveManager.Instance.CompleteLevel(nextLevelToUnlock, currentCoins);
            }

            SceneController.instance?.LoadNextLevel();
        }
    }
}