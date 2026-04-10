using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    public void Initialize(int maxHealth, int currentHealth)
    {
        slider.minValue = 0;
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

    public void SetHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
    }
}