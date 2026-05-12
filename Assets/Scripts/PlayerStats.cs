using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Survivability")]
    public int maxHealth = 10;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Combat")]
    public int damage = 1;
    public float attackInterval = 0.7f;
    public float attackRange = 6f;
    public float bulletSpeed = 10f;
    public float bulletSize = 1f;

    [Header("Projectiles")]
    public int projectileCount = 1;
    public float projectileSpread = 0f;
    public int pierceCount = 0;

    [Header("Critical Hits")]
    [Range(0f, 1f)]
    public float critChance = 0.05f;
    public float critMultiplier = 2f;

    [Header("Life Steal")]
    [Range(0f, 1f)]
    public float lifeStealChance = 0f;

    [Header("Pickup")]
    public float pickupRange = 1.5f;
}
