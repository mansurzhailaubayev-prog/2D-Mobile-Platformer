using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [Header("UI Pause Panel")]
    public GameObject pausePanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Отписываемся при уничтожении
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Этот метод вызывается АВТОМАТИЧЕСКИ при каждом переходе и рестарте
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f; // Всегда сбрасываем паузу при загрузке новой сцены

        // Ищем панель паузы на новой сцене автоматически по имени
        GameObject foundPanel = GameObject.Find("PausePanel");
        if (foundPanel != null)
        {
            pausePanel = foundPanel;
            pausePanel.SetActive(false); // Гарантированно скрываем при старте
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene("WinScreen");
    }

    public void RestartCurrentLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        // Если ссылка потерялась при переходе, ищем панель повторно
        if (pausePanel == null)
        {
            pausePanel = GameObject.Find("PausePanel");
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }
}