using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Vector3 localOffset = new Vector3(0f, 0.7f, 0f);
    [SerializeField] private float width = 0.8f;
    [SerializeField] private float height = 0.08f;
    [SerializeField] private Color healthColor = Color.red;
    [SerializeField] private Color missingHealthColor = Color.green;
    [SerializeField] private int sortingOrder = 20;

    private static Sprite whiteSprite;

    private Transform healthFill;
    private SpriteRenderer missingRenderer;
    private SpriteRenderer healthRenderer;

    private void Awake()
    {
        CreateBar();
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        if (healthFill == null) return;

        float healthPercent = maxHealth > 0
            ? Mathf.Clamp01((float)currentHealth / maxHealth)
            : 0f;

        float fillWidth = width * healthPercent;
        healthFill.localScale = new Vector3(fillWidth, height, 1f);
        healthFill.localPosition = localOffset + new Vector3((-width + fillWidth) * 0.5f, 0f, -0.01f);

        bool shouldShow = currentHealth > 0 && currentHealth < maxHealth;
        missingRenderer.enabled = shouldShow;
        healthRenderer.enabled = shouldShow;
    }

    private void CreateBar()
    {
        Sprite sprite = GetWhiteSprite();

        Transform missingFill = CreateBarPart("MissingHealth", sprite, missingHealthColor, sortingOrder, out missingRenderer);
        missingFill.localPosition = localOffset;
        missingFill.localScale = new Vector3(width, height, 1f);

        healthFill = CreateBarPart("Health", sprite, healthColor, sortingOrder + 1, out healthRenderer);
        SetHealth(1, 1);
    }

    private Transform CreateBarPart(string partName, Sprite sprite, Color color, int order, out SpriteRenderer renderer)
    {
        GameObject part = new GameObject(partName);
        part.transform.SetParent(transform);
        part.transform.localRotation = Quaternion.identity;

        renderer = part.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = color;
        renderer.sortingOrder = order;

        return part.transform;
    }

    private static Sprite GetWhiteSprite()
    {
        if (whiteSprite != null)
        {
            return whiteSprite;
        }

        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        whiteSprite = Sprite.Create(
            texture,
            new Rect(0f, 0f, 1f, 1f),
            new Vector2(0.5f, 0.5f),
            1f
        );

        return whiteSprite;
    }
}
