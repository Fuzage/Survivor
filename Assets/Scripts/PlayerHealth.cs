using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float invincibleTime = 0.5f;
    [SerializeField] private HealthBarUI healthBarUI;

    private PlayerStats stats;
    private int currentHealth;
    private float invincibleTimer;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        if (stats == null)
        {
            stats = gameObject.AddComponent<PlayerStats>();
        }
    }

    private void Start()
    {
        currentHealth = GetMaxHealth();

        if (healthBarUI != null)
        {
            healthBarUI.Initialize(GetMaxHealth(), currentHealth);
        }
    }

    private void Update()
    {
        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        if (invincibleTimer > 0f) return;

        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        invincibleTimer = invincibleTime;

        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(currentHealth);
        }

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        Destroy(gameObject);
    }

    public void IncreaseMaxHealth(int amount)
    {
        if (stats != null)
        {
            stats.maxHealth += amount;
        }

        currentHealth += amount;

        currentHealth = Mathf.Min(currentHealth, GetMaxHealth());

        if (healthBarUI != null)
        {
            healthBarUI.SetMaxHealth(GetMaxHealth());
            healthBarUI.SetHealth(currentHealth);
        }

        Debug.Log("Max HP increased to: " + GetMaxHealth());
    }

    public bool Heal(int amount)
    {
        if (amount <= 0 || currentHealth <= 0 || currentHealth >= GetMaxHealth()) return false;

        currentHealth = Mathf.Min(currentHealth + amount, GetMaxHealth());

        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(currentHealth);
        }

        return true;
    }

    private int GetMaxHealth()
    {
        return stats != null ? stats.maxHealth : 10;
    }
}
