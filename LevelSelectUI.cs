using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectUI : MonoBehaviour
{
    [Header("Level Buttons & Locks")]
    public Button[] levelButtons; // Перетащи сюда кнопки: Level 1, Level 2, Level 3
    public GameObject[] lockIcons; // Перетащи сюда иконки замков (для 1-го уровня можно оставить пустым или null)

    [Header("UI & Settings")]
    public TextMeshProUGUI bestScoreText;
    public Slider volumeSlider;
    public GameObject confirmResetPanel; // Панель подтверждения "Вы уверены?"

    private void Start()
    {
        if (confirmResetPanel != null) confirmResetPanel.SetActive(false);
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (SaveManager.Instance == null) return;

        GameData data = SaveManager.Instance.data;

        if (bestScoreText != null)
            bestScoreText.text = "Best Score: " + data.bestScore;

        if (volumeSlider != null)
            volumeSlider.value = data.musicVolume;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNum = i + 1;
            bool isUnlocked = data.unlockedLevels.Contains(levelNum);

            if (levelButtons[i] != null)
                levelButtons[i].interactable = isUnlocked;

            if (i < lockIcons.Length && lockIcons[i] != null)
                lockIcons[i].SetActive(!isUnlocked);
        }
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnVolumeChanged(float val)
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.SetVolume(val);
    }

    public void OpenResetConfirm()
    {
        if (confirmResetPanel != null) confirmResetPanel.SetActive(true);
    }

    public void CloseResetConfirm()
    {
        if (confirmResetPanel != null) confirmResetPanel.SetActive(false);
    }

    public void ConfirmResetProgress()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ResetProgress();
            RefreshUI();
        }
        CloseResetConfirm();
    }
}