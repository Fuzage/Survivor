using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int maxHealth = 3;
    public float moveSpeed = 4f;
    public int contactDamage = 1;

    public void ApplyDifficulty(EnemyDifficultyManager difficultyManager)
    {
        if (difficultyManager == null) return;

        maxHealth = Mathf.Max(1, Mathf.RoundToInt(maxHealth * difficultyManager.HealthMultiplier));
        moveSpeed *= difficultyManager.MoveSpeedMultiplier;
        contactDamage = Mathf.Max(1, Mathf.RoundToInt(contactDamage * difficultyManager.DamageMultiplier));
    }
}
