using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    [Header("Gun Settings")]
    [SerializeField] private float fireInterval = 0.7f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float bulletSpeed = 10f;

    private float fireTimer;

    private void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer > 0f) return;

        Transform nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return;

        Shoot(nearestEnemy);
        fireTimer = fireInterval;
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance > attackRange) continue;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }

    private void Shoot(Transform target)
    {
        Vector2 baseDirection = (target.position - transform.position).normalized;

        FireSingleBullet(baseDirection);
    }

    private void FireSingleBullet(Vector2 direction)
    {
        if (bulletPrefab == null) return;

        GameObject bulletObject = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Init(direction, damage, bulletSpeed, attackRange);
        }
    }
}