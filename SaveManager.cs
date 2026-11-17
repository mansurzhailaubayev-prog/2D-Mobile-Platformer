using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameData
{
    public int version = 1;
    public int bestScore = 0;
    public List<int> unlockedLevels = new List<int> { 1 };
    public float musicVolume = 0.8f;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public GameData data = new GameData();

    private string saveFilePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        LoadGame();
    }

    public void SaveGame()
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Game saved to: " + saveFilePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save game: " + e.Message);
        }
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("Save file not found. Creating defaults.");
            data = new GameData();
            SaveGame();
            return;
        }

        try
        {
            string json = File.ReadAllText(saveFilePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(json);

            if (loadedData == null || loadedData.unlockedLevels == null || loadedData.unlockedLevels.Count == 0)
            {
                throw new Exception("Corrupted or empty JSON structure.");
            }

            data = loadedData;
            AudioListener.volume = data.musicVolume;
            Debug.Log("Save loaded successfully.");
        }
        catch (Exception e)
        {
            Debug.LogWarning("Broken save file detected! Falling back to defaults. Error: " + e.Message);
            data = new GameData();
            AudioListener.volume = data.musicVolume;
        }
    }

    public void CompleteLevel(int nextLevelNumber, int currentScore)
    {
        if (currentScore > data.bestScore)
        {
            data.bestScore = currentScore;
        }

        if (!data.unlockedLevels.Contains(nextLevelNumber))
        {
            data.unlockedLevels.Add(nextLevelNumber);
        }

        SaveGame();
    }

    public void UnlockLevel(int levelNumber)
    {
        if (!data.unlockedLevels.Contains(levelNumber))
        {
            data.unlockedLevels.Add(levelNumber);
            SaveGame();
        }
    }

    public void SetVolume(float volume)
    {
        data.musicVolume = volume;
        AudioListener.volume = volume;
        SaveGame();
    }

    public void ResetProgress()
    {
        data = new GameData();
        AudioListener.volume = data.musicVolume;
        SaveGame();
        Debug.Log("Progress reset to defaults.");
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGame();
        }
    }
}