using UnityEngine;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private TextMeshProUGUI timerText;

    private float remainingTime;

    private void Start()
    {
        if (levelData == null) return;

        remainingTime = levelData.timeLimit;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = levelData.playerStartPosition;
        }

        foreach (EnemySpawnInfo info in levelData.enemiesToSpawn)
        {
            if (info.enemyPrefab != null)
            {
                Instantiate(info.enemyPrefab, info.spawnPosition, Quaternion.identity);
            }
        }
    }

    private void Update()
    {
        if (levelData == null || timerText == null) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime).ToString();
        }
    }
}