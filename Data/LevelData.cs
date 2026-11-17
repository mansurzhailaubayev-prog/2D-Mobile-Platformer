using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public Vector2 spawnPosition;
}

[CreateAssetMenu(fileName = "Level_01", menuName = "Platformer Data/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelNumber = 1;
    public string levelName = "Level 1";
    public float timeLimit = 90f;
    public Vector2 playerStartPosition = new Vector2(-6f, 0f);
    public List<EnemySpawnInfo> enemiesToSpawn = new List<EnemySpawnInfo>();
}