using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentExp = 0;
    [SerializeField] private int expToNextLevel = 5;
    [SerializeField] private ExperienceUI experienceUI;

    private void Start()
    {
        RefreshUI();
    }

    public void AddExperience(int amount)
    {
        currentExp += amount;

        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        }

        RefreshUI();
        Debug.Log("EXP: " + currentExp + " / " + expToNextLevel);
    }


    private void RefreshUI()
    {
        if (experienceUI != null)
        {
            experienceUI.UpdateLevel(currentLevel);
            experienceUI.UpdateExperience(currentExp, expToNextLevel);
        }
    }

    private void LevelUp()
    {
        currentExp -= expToNextLevel;
        currentLevel++;
        expToNextLevel += 3;

        Debug.Log("Level Up! Current Level: " + currentLevel);

        // 👉 升级奖励：+1 最大血量
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(1);
        }
    }
}