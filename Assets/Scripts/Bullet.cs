using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 moveDirection;
    private Vector2 startPosition;

    private int damage;
    private float moveSpeed;
    private float maxRange;
    private bool isCriticalHit;
    private PlayerStats ownerStats;
    private PlayerHealth ownerHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    public void Init(Vector2 direction, int damageValue, float speedValue, float rangeValue)
    {
        Init(direction, damageValue, speedValue, rangeValue, false);
    }

    public void Init(Vector2 direction, int damageValue, float speedValue, float rangeValue, bool criticalHit)
    {
        moveDirection = direction.normalized;
        damage = damageValue;
        moveSpeed = speedValue;
        maxRange = rangeValue;
        isCriticalHit = criticalHit;
        startPosition = rb.position;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void SetOwner(PlayerStats stats, PlayerHealth health)
    {
        ownerStats = stats;
        ownerHealth = health;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;

        float traveledDistance = Vector2.Distance(startPosition, rb.position);
        if (traveledDistance >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            ShowCriticalText(other.transform.position);
            TryLifeSteal();
        }

        Destroy(gameObject);
    }

    private void ShowCriticalText(Vector3 enemyPosition)
    {
        if (!isCriticalHit) return;

        Vector3 textPosition = enemyPosition + Vector3.up * 0.7f + GetTextJitter(0.2f);
        FloatingText.Show(damage.ToString(), textPosition, Color.red, 6f);
    }

    private void TryLifeSteal()
    {
        if (ownerStats == null || ownerHealth == null) return;

        if (Random.value < ownerStats.lifeStealChance)
        {
            if (ownerHealth.Heal(1))
            {
                Vector3 textPosition = ownerHealth.transform.position + Vector3.up * 0.8f + GetTextJitter(0.25f);
                FloatingText.Show("+1", textPosition, new Color(0.25f, 1f, 0.35f), 5f);
            }
        }
    }

    private Vector3 GetTextJitter(float radius)
    {
        return new Vector3(Random.Range(-radius, radius), Random.Range(-radius * 0.5f, radius), 0f);
    }
}
