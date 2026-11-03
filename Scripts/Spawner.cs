using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawnPrefabs; // Массив префабов (Монета, Сердце)
    public float spawnInterval = 3f;
    public float spawnRangeX = 6f;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), 1f, spawnInterval);
    }

    void Spawn()
    {
        if (spawnPrefabs == null || spawnPrefabs.Length == 0) return;

        GameObject selectedPrefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(transform.position.x + randomX, transform.position.y, 0);

        Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
    }
}