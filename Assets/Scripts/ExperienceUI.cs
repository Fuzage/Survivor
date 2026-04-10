using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Slider expSlider;

    private void Awake()
    {
        if (expSlider != null)
        {
            expSlider.minValue = 0;
        }
    }

    public void UpdateLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = "LV." + level;
        }
    }

    public void UpdateExperience(int currentExp, int expToNextLevel)
    {
        if (expSlider != null)
        {
            expSlider.maxValue = expToNextLevel;
            expSlider.value = currentExp;
        }
    }
}