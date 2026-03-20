using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float minSpawnDistance = 8f;
    [SerializeField] private float maxSpawnDistance = 10f;

    private float spawnTimer;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || player == null)
        {
            Debug.LogWarning("EnemySpawner is missing enemyPrefab or player reference.");
            return;
        }

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector2 spawnPosition = (Vector2)player.position + randomDirection * randomDistance;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}