using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private struct UpgradeOption
    {
        public string title;
        public string description;
        public Action apply;
    }

    [SerializeField] private int choicesPerLevel = 3;
    [SerializeField] private Vector2 panelSize = new Vector2(620f, 260f);
    [SerializeField] private Vector2 buttonSize = new Vector2(180f, 170f);

    private PlayerStats stats;
    private PlayerHealth playerHealth;
    private GameObject upgradePanel;
    private readonly List<UpgradeOption> optionPool = new List<UpgradeOption>();
    private int pendingUpgradeChoices;
    private bool isShowing;
    private float previousTimeScale = 1f;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void ShowUpgradeChoices()
    {
        if (stats == null)
        {
            Debug.LogWarning("UpgradeManager could not find PlayerStats.");
            return;
        }

        if (isShowing)
        {
            pendingUpgradeChoices++;
            return;
        }

        isShowing = true;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        BuildOptionPool();
        CreatePanel();
        PopulateChoices();
    }

    private void BuildOptionPool()
    {
        optionPool.Clear();

        optionPool.Add(new UpgradeOption
        {
            title = "Sharpened Arrow",
            description = "+1 Damage",
            apply = () => stats.damage += 1
        });

        optionPool.Add(new UpgradeOption
        {
            title = "Quick Draw",
            description = "+15% Attack Speed",
            apply = () => stats.attackInterval = Mathf.Max(0.15f, stats.attackInterval * 0.85f)
        });

        optionPool.Add(new UpgradeOption
        {
            title = "Fleet Step",
            description = "+10% Move Speed",
            apply = () => stats.moveSpeed *= 1.1f
        });

        optionPool.Add(new UpgradeOption
        {
            title = "Wider Reach",
            description = "+25% Pickup Range",
            apply = () => stats.pickupRange *= 1.25f
        });

        optionPool.Add(new UpgradeOption
        {
            title = "Vitality",
            description = "+2 Max Health",
            apply = () =>
            {
                if (playerHealth != null)
                {
                    playerHealth.IncreaseMaxHealth(2);
                }
                else
                {
                    stats.maxHealth += 2;
                }
            }
        });

        optionPool.Add(new UpgradeOption
        {
            title = "Long Shot",
            description = "+20% Attack Range",
            apply = () => stats.attackRange *= 1.2f
        });

        optionPool.Add(new UpgradeOption
        {
            title = "Swift Arrow",
            description = "+20% Bullet Speed",
            apply = () => stats.bulletSpeed *= 1.2f
        });

        optionPool.Add(new UpgradeOption
        {
            title = "Heavy Arrow",
            description = "+15% Bullet Size",
            apply = () => stats.bulletSize *= 1.15f
        });

        optionPool.Add(new UpgradeOption
        {
            title = "Keen Eye",
            description = "+8% Crit Chance",
            apply = () => stats.critChance = Mathf.Min(1f, stats.critChance + 0.08f)
        });

        optionPool.Add(new UpgradeOption
        {
            title = "Executioner",
            description = "+35% Crit Damage",
            apply = () => stats.critMultiplier += 0.35f
        });

        optionPool.Add(new UpgradeOption
        {
            title = "Blood Tithe",
            description = "+6% chance to heal 1 HP on hit",
            apply = () => stats.lifeStealChance = Mathf.Min(1f, stats.lifeStealChance + 0.06f)
        });
    }

    private void CreatePanel()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("UpgradeManager could not find a Canvas.");
            return;
        }

        if (upgradePanel != null)
        {
            Destroy(upgradePanel);
        }

        upgradePanel = new GameObject("UpgradePanel");
        upgradePanel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = upgradePanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = panelSize;

        Image panelImage = upgradePanel.AddComponent<Image>();
        panelImage.color = new Color(0.04f, 0.04f, 0.06f, 0.92f);

        CreateTitle(panelRect);
    }

    private void CreateTitle(RectTransform panelRect)
    {
        TextMeshProUGUI title = CreateText("Choose an Upgrade", upgradePanel.transform, 30, FontStyles.Bold);
        RectTransform titleRect = title.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -18f);
        titleRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, 44f);
        title.alignment = TextAlignmentOptions.Center;
    }

    private void PopulateChoices()
    {
        ShuffleOptions();

        int choiceCount = Mathf.Min(choicesPerLevel, optionPool.Count);
        float spacing = 24f;
        float totalWidth = choiceCount * buttonSize.x + (choiceCount - 1) * spacing;
        float startX = -totalWidth * 0.5f + buttonSize.x * 0.5f;

        for (int i = 0; i < choiceCount; i++)
        {
            UpgradeOption option = optionPool[i];

            CreateChoiceButton(option, new Vector2(startX + i * (buttonSize.x + spacing), -30f));
        }
    }

    private void ShuffleOptions()
    {
        for (int i = optionPool.Count - 1; i > 0; i--)
        {
            int swapIndex = UnityEngine.Random.Range(0, i + 1);
            (optionPool[i], optionPool[swapIndex]) = (optionPool[swapIndex], optionPool[i]);
        }
    }

    private void CreateChoiceButton(UpgradeOption option, Vector2 position)
    {
        GameObject buttonObject = new GameObject(option.title);
        buttonObject.transform.SetParent(upgradePanel.transform, false);

        RectTransform buttonRect = buttonObject.AddComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.anchoredPosition = position;
        buttonRect.sizeDelta = buttonSize;

        Image buttonImage = buttonObject.AddComponent<Image>();
        buttonImage.color = new Color(0.12f, 0.12f, 0.16f, 0.96f);

        Button button = buttonObject.AddComponent<Button>();
        button.onClick.AddListener(() => SelectUpgrade(option));

        TextMeshProUGUI titleText = CreateText(option.title, buttonObject.transform, 20, FontStyles.Bold);
        RectTransform titleRect = titleText.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -18f);
        titleRect.sizeDelta = new Vector2(buttonSize.x - 24f, 54f);
        titleText.alignment = TextAlignmentOptions.Center;

        TextMeshProUGUI descriptionText = CreateText(option.description, buttonObject.transform, 17, FontStyles.Normal);
        RectTransform descriptionRect = descriptionText.GetComponent<RectTransform>();
        descriptionRect.anchorMin = new Vector2(0.5f, 0.5f);
        descriptionRect.anchorMax = new Vector2(0.5f, 0.5f);
        descriptionRect.pivot = new Vector2(0.5f, 0.5f);
        descriptionRect.anchoredPosition = new Vector2(0f, -20f);
        descriptionRect.sizeDelta = new Vector2(buttonSize.x - 24f, 80f);
        descriptionText.alignment = TextAlignmentOptions.Center;
    }

    private TextMeshProUGUI CreateText(string text, Transform parent, int fontSize, FontStyles fontStyle)
    {
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(parent, false);

        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.fontStyle = fontStyle;
        textComponent.color = Color.white;
        textComponent.textWrappingMode = TextWrappingModes.Normal;

        return textComponent;
    }

    private void SelectUpgrade(UpgradeOption option)
    {
        option.apply?.Invoke();

        if (upgradePanel != null)
        {
            Destroy(upgradePanel);
        }

        if (pendingUpgradeChoices > 0)
        {
            pendingUpgradeChoices--;
            isShowing = false;
            ShowUpgradeChoices();
            return;
        }

        Time.timeScale = previousTimeScale;
        isShowing = false;
    }
}
