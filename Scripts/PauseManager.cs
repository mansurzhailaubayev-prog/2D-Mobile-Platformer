using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Перетащи сюда панель меню паузы из Canvas")]
    public GameObject pauseMenuPanel;

    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1f;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        AudioManager.Instance?.PlayUIClick();
        AudioManager.Instance?.SetPausedSnapshot(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        AudioManager.Instance?.PlayUIClick();
        AudioManager.Instance?.SetPausedSnapshot(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.PlayUIClick();
        AudioManager.Instance?.SetPausedSnapshot(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.PlayUIClick();
        AudioManager.Instance?.SetPausedSnapshot(false);
        SceneManager.LoadScene("MainMenu");
    }
}