using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float baseMoveSpeedForAttackBonus = 5f;
    [SerializeField] private float attackSpeedBonusPerMoveSpeed = 0.2f;
    [SerializeField] private float maxMoveSpeedAttackBonus = 1f;

    private PlayerStats stats;
    private PlayerHealth health;
    private float fireTimer;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        if (stats == null)
        {
            stats = gameObject.AddComponent<PlayerStats>();
        }

        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer > 0f) return;

        Transform nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return;

        Shoot(nearestEnemy);
        fireTimer = GetAttackInterval();
    }

    private Transform FindNearestEnemy()
    {
        Transform nearestEnemy = null;
        float nearestDistanceSqr = Mathf.Infinity;
        float attackRange = GetAttackRange();
        float attackRangeSqr = attackRange * attackRange;
        Vector3 playerPosition = transform.position;

        for (int i = 0; i < EnemyRegistry.Count; i++)
        {
            EnemyController enemy = EnemyRegistry.GetEnemy(i);
            if (enemy == null || !enemy.isActiveAndEnabled) continue;

            Vector3 offset = enemy.transform.position - playerPosition;
            float distanceSqr = offset.sqrMagnitude;

            if (distanceSqr > attackRangeSqr) continue;

            if (distanceSqr < nearestDistanceSqr)
            {
                nearestDistanceSqr = distanceSqr;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }

    private void Shoot(Transform target)
    {
        Vector2 baseDirection = (target.position - transform.position).normalized;
        int projectileCount = GetProjectileCount();

        if (projectileCount <= 1)
        {
            FireSingleBullet(baseDirection);
            return;
        }

        float spread = GetProjectileSpread();
        float startAngle = -spread * (projectileCount - 1) * 0.5f;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + spread * i;
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * baseDirection;
            FireSingleBullet(direction);
        }
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
            int damage = RollDamage(out bool isCriticalHit);
            bullet.Init(direction, damage, GetBulletSpeed(), GetAttackRange(), isCriticalHit);
            bullet.SetOwner(stats, health);
        }

        bulletObject.transform.localScale *= GetBulletSize();
    }

    private int RollDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;
        int damage = stats != null ? stats.damage : 1;
        if (stats != null && Random.value < stats.critChance)
        {
            isCriticalHit = true;
            damage = Mathf.RoundToInt(damage * stats.critMultiplier);
        }

        return Mathf.Max(1, damage);
    }

    private float GetAttackInterval()
    {
        if (stats == null)
        {
            return 0.7f;
        }

        float extraMoveSpeed = Mathf.Max(0f, stats.moveSpeed - baseMoveSpeedForAttackBonus);
        float attackSpeedBonus = Mathf.Min(maxMoveSpeedAttackBonus, extraMoveSpeed * attackSpeedBonusPerMoveSpeed);
        return stats.attackInterval / (1f + attackSpeedBonus);
    }

    private float GetAttackRange()
    {
        return stats != null ? stats.attackRange : 6f;
    }

    private float GetBulletSpeed()
    {
        return stats != null ? stats.bulletSpeed : 10f;
    }

    private float GetBulletSize()
    {
        return stats != null ? stats.bulletSize : 1f;
    }

    private int GetProjectileCount()
    {
        return stats != null ? Mathf.Max(1, stats.projectileCount) : 1;
    }

    private float GetProjectileSpread()
    {
        return stats != null ? stats.projectileSpread : 0f;
    }
}
