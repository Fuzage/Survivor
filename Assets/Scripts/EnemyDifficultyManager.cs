using UnityEngine;

public class EnemyDifficultyManager : MonoBehaviour
{
    public static EnemyDifficultyManager Instance { get; private set; }

    [Header("Time Scaling")]
    [SerializeField] private float healthMultiplierPerMinute = 0.45f;
    [SerializeField] private float moveSpeedMultiplierPerMinute = 0.03f;
    [SerializeField] private float damageMultiplierPerMinute = 0.15f;
    [SerializeField] private float spawnRateMultiplierPerMinute = 0.45f;
    [SerializeField] private float pressureStartMinute = 8f;
    [SerializeField] private float pressureRampMinutes = 4f;
    [SerializeField] private float pressureHealthMultiplier = 4f;
    [SerializeField] private float pressureSpawnRateMultiplier = 2.5f;
    [SerializeField] private float pressureDamageMultiplier = 1.4f;

    [Header("Time Scaling Caps")]
    [SerializeField] private float maxHealthTimeMultiplier = 18f;
    [SerializeField] private float maxMoveSpeedTimeMultiplier = 1.5f;
    [SerializeField] private float maxDamageTimeMultiplier = 4f;
    [SerializeField] private float maxSpawnRateTimeMultiplier = 8f;
    [SerializeField] private float minSpawnIntervalMultiplier = 0.08f;

    [Header("Relic Modifiers")]
    [SerializeField] private float relicHealthMultiplier = 1f;
    [SerializeField] private float relicMoveSpeedMultiplier = 1f;
    [SerializeField] private float relicDamageMultiplier = 1f;
    [SerializeField] private float relicSpawnRateMultiplier = 1f;

    private float elapsedTime;

    public float HealthMultiplier => Mathf.Min(maxHealthTimeMultiplier, GetTimeMultiplier(healthMultiplierPerMinute) * GetPressureMultiplier(pressureHealthMultiplier)) * relicHealthMultiplier;
    public float MoveSpeedMultiplier => GetTimeMultiplier(moveSpeedMultiplierPerMinute, maxMoveSpeedTimeMultiplier) * relicMoveSpeedMultiplier;
    public float DamageMultiplier => Mathf.Min(maxDamageTimeMultiplier, GetTimeMultiplier(damageMultiplierPerMinute) * GetPressureMultiplier(pressureDamageMultiplier)) * relicDamageMultiplier;

    public float SpawnIntervalMultiplier
    {
        get
        {
            float spawnRateMultiplier = Mathf.Min(maxSpawnRateTimeMultiplier, GetTimeMultiplier(spawnRateMultiplierPerMinute) * GetPressureMultiplier(pressureSpawnRateMultiplier)) * relicSpawnRateMultiplier;
            return Mathf.Max(minSpawnIntervalMultiplier, 1f / spawnRateMultiplier);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void MultiplyEnemyHealth(float multiplier)
    {
        relicHealthMultiplier *= multiplier;
    }

    public void MultiplyEnemyMoveSpeed(float multiplier)
    {
        relicMoveSpeedMultiplier *= multiplier;
    }

    public void MultiplyEnemyDamage(float multiplier)
    {
        relicDamageMultiplier *= multiplier;
    }

    public void MultiplyEnemySpawnRate(float multiplier)
    {
        relicSpawnRateMultiplier *= multiplier;
    }

    private float GetTimeMultiplier(float multiplierPerMinute, float maxMultiplier)
    {
        float elapsedMinutes = elapsedTime / 60f;
        return Mathf.Min(maxMultiplier, 1f + elapsedMinutes * multiplierPerMinute);
    }

    private float GetTimeMultiplier(float multiplierPerMinute)
    {
        float elapsedMinutes = elapsedTime / 60f;
        return 1f + elapsedMinutes * multiplierPerMinute;
    }

    private float GetPressureMultiplier(float targetMultiplier)
    {
        float elapsedMinutes = elapsedTime / 60f;
        float pressureProgress = Mathf.InverseLerp(
            pressureStartMinute,
            pressureStartMinute + pressureRampMinutes,
            elapsedMinutes
        );

        return Mathf.Lerp(1f, targetMultiplier, pressureProgress);
    }
}
