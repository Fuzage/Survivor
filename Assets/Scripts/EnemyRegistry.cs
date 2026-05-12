using System.Collections.Generic;

public static class EnemyRegistry
{
    private static readonly List<EnemyController> activeEnemies = new List<EnemyController>(512);

    public static int Count => activeEnemies.Count;

    public static void Register(EnemyController enemy)
    {
        if (enemy == null || activeEnemies.Contains(enemy)) return;

        activeEnemies.Add(enemy);
    }

    public static void Unregister(EnemyController enemy)
    {
        if (enemy == null) return;

        activeEnemies.Remove(enemy);
    }

    public static EnemyController GetEnemy(int index)
    {
        return activeEnemies[index];
    }
}
