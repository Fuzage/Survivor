using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float minSpawnDistance = 8f;
    [SerializeField] private float maxSpawnDistance = 10f;
    [SerializeField] private float mapEdgePadding = 2f;
    [SerializeField] private int spawnAttempts = 20;

    private float spawnTimer;
    private EnemyDifficultyManager difficultyManager;

    private void Awake()
    {
        difficultyManager = GetComponent<EnemyDifficultyManager>();
        if (difficultyManager == null)
        {
            difficultyManager = gameObject.AddComponent<EnemyDifficultyManager>();
        }
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = GetCurrentSpawnInterval();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || player == null)
        {
            Debug.LogWarning("EnemySpawner is missing enemyPrefab or player reference.");
            return;
        }

        Vector2 spawnPosition = GetSpawnPosition();

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 playerPosition = player.position;

        for (int i = 0; i < spawnAttempts; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            if (randomDirection == Vector2.zero)
            {
                randomDirection = Vector2.right;
            }

            float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector2 candidate = playerPosition + randomDirection * randomDistance;

            if (IsValidSpawnPosition(candidate, playerPosition))
            {
                return candidate;
            }
        }

        if (MapArea.Instance != null)
        {
            return MapArea.Instance.GetRandomPoint(mapEdgePadding);
        }

        return playerPosition + Random.insideUnitCircle.normalized * maxSpawnDistance;
    }

    private bool IsValidSpawnPosition(Vector2 candidate, Vector2 playerPosition)
    {
        float distanceToPlayer = Vector2.Distance(candidate, playerPosition);
        if (distanceToPlayer < minSpawnDistance || distanceToPlayer > maxSpawnDistance)
        {
            return false;
        }

        if (MapArea.Instance == null)
        {
            return true;
        }

        Vector2 min = MapArea.Instance.Min + Vector2.one * mapEdgePadding;
        Vector2 max = MapArea.Instance.Max - Vector2.one * mapEdgePadding;

        return candidate.x >= min.x
            && candidate.x <= max.x
            && candidate.y >= min.y
            && candidate.y <= max.y;
    }

    private float GetCurrentSpawnInterval()
    {
        float intervalMultiplier = difficultyManager != null ? difficultyManager.SpawnIntervalMultiplier : 1f;
        return spawnInterval * intervalMultiplier;
    }
}
