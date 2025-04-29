using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Spawn Timing")]
    public float spawnInterval = 2f;
    public float minSpawnInterval = 0.5f;

    [Header("Difficulty Scaling")]
    public float difficultyIncreaseRate = 5f;
    public float spawnDecreaseAmount = 0.2f;

    private float spawnTimer = 0f;
    private float difficultyTimer = 0f;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }

        if (difficultyTimer >= difficultyIncreaseRate)
        {
            IncreaseDifficulty();
            difficultyTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[index].position, enemyPrefab.transform.rotation);
    }

    void IncreaseDifficulty()
    {
        spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - spawnDecreaseAmount);
        Debug.Log($"Spawn interval decreased to: {spawnInterval}");
    }
}