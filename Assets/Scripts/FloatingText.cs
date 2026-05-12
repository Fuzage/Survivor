using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private const float Lifetime = 0.75f;
    private const float FloatSpeed = 1.4f;

    private TextMeshPro textMesh;
    private Color startColor;
    private float age;

    public static void Show(string text, Vector3 position, Color color, float fontSize = 5f)
    {
        GameObject textObject = new GameObject("FloatingText");
        textObject.transform.position = position;

        FloatingText floatingText = textObject.AddComponent<FloatingText>();
        floatingText.Initialize(text, color, fontSize);
    }

    private void Initialize(string text, Color color, float fontSize)
    {
        textMesh = gameObject.AddComponent<TextMeshPro>();
        textMesh.text = text;
        textMesh.color = color;
        textMesh.fontSize = fontSize;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.sortingOrder = 100;

        startColor = color;
    }

    private void Update()
    {
        age += Time.deltaTime;

        transform.position += Vector3.up * (FloatSpeed * Time.deltaTime);

        float t = Mathf.Clamp01(age / Lifetime);
        Color fadedColor = startColor;
        fadedColor.a = 1f - t;
        textMesh.color = fadedColor;

        if (age >= Lifetime)
        {
            Destroy(gameObject);
        }
    }
}
