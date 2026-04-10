using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float invincibleTime = 0.5f;
    [SerializeField] private HealthBarUI healthBarUI;

    private int currentHealth;
    private float invincibleTimer;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBarUI != null)
        {
            healthBarUI.Initialize(maxHealth, currentHealth);
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
        maxHealth += amount;
        currentHealth += amount;

        // ✅ 防止超过最大值（以后会用到）
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (healthBarUI != null)
        {
            healthBarUI.SetMaxHealth(maxHealth);
            healthBarUI.SetHealth(currentHealth);
        }

        Debug.Log("Max HP increased to: " + maxHealth);
    }
}