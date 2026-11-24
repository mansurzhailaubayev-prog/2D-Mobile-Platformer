using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectUI : MonoBehaviour
{
    [Header("Level Buttons & Locks")]
    public Button[] levelButtons;
    public GameObject[] lockIcons;

    [Header("UI & Settings")]
    public TextMeshProUGUI bestScoreText;
    public Slider musicSlider;
    public Slider sfxSlider;
    public GameObject confirmResetPanel;

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

        if (musicSlider != null)
            musicSlider.value = data.musicVolume;

        if (sfxSlider != null)
            sfxSlider.value = data.sfxVolume;

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
        AudioManager.Instance?.PlayUIClick();
        SceneManager.LoadScene(sceneName);
    }

    public void OnMusicVolumeChanged(float val)
    {
        AudioManager.Instance?.ApplyMusicVolume(val);
        SaveManager.Instance?.SetVolume(val);
    }

    public void OnSFXVolumeChanged(float val)
    {
        AudioManager.Instance?.ApplySFXVolume(val);
        SaveManager.Instance?.SetSFXVolume(val);
    }

    public void OpenResetConfirm()
    {
        AudioManager.Instance?.PlayUIClick();
        if (confirmResetPanel != null) confirmResetPanel.SetActive(true);
    }

    public void CloseResetConfirm()
    {
        AudioManager.Instance?.PlayUIClick();
        if (confirmResetPanel != null) confirmResetPanel.SetActive(false);
    }

    public void ConfirmResetProgress()
    {
        AudioManager.Instance?.PlayUIClick();
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ResetProgress();
            AudioManager.Instance?.ApplyMusicVolume(SaveManager.Instance.data.musicVolume);
            AudioManager.Instance?.ApplySFXVolume(SaveManager.Instance.data.sfxVolume);
            RefreshUI();
        }
        CloseResetConfirm();
    }
}