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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    public void Init(Vector2 direction, int damageValue, float speedValue, float rangeValue)
    {
        moveDirection = direction.normalized;
        damage = damageValue;
        moveSpeed = speedValue;
        maxRange = rangeValue;
        startPosition = rb.position;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
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
        }

        Destroy(gameObject);
    }
}