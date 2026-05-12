using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject expOrbPrefab;

    private EnemyStats stats;
    private int currentHealth;
    private EnemyHealthBar healthBar;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        if (stats == null)
        {
            stats = gameObject.AddComponent<EnemyStats>();
        }

        stats.ApplyDifficulty(EnemyDifficultyManager.Instance);
    }

    private void Start()
    {
        currentHealth = stats.maxHealth;
        healthBar = GetComponent<EnemyHealthBar>();
        if (healthBar == null)
        {
            healthBar = gameObject.AddComponent<EnemyHealthBar>();
        }

        healthBar.SetHealth(currentHealth, stats.maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, stats.maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (expOrbPrefab != null)
        {
            Instantiate(expOrbPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
