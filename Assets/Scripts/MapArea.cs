using UnityEngine;

public class MapArea : MonoBehaviour
{
    public static MapArea Instance { get; private set; }

    [Header("Size")]
    [SerializeField] private Vector2 mapSize = new Vector2(120f, 80f);
    [SerializeField] private float wallThickness = 1f;

    [Header("Background")]
    [SerializeField] private Sprite backgroundSpriteAsset;
    [SerializeField] private int backgroundSortingOrder = -100;

    public Vector2 Size => mapSize;
    public Vector2 Min => (Vector2)transform.position - mapSize * 0.5f;
    public Vector2 Max => (Vector2)transform.position + mapSize * 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CreateBackground();
        CreateBoundary();
    }

    public Vector2 ClampPoint(Vector2 point, float padding = 0f)
    {
        Vector2 min = Min + Vector2.one * padding;
        Vector2 max = Max - Vector2.one * padding;

        return new Vector2(
            Mathf.Clamp(point.x, min.x, max.x),
            Mathf.Clamp(point.y, min.y, max.y)
        );
    }

    public Vector2 GetRandomPoint(float padding = 0f)
    {
        Vector2 min = Min + Vector2.one * padding;
        Vector2 max = Max - Vector2.one * padding;

        return new Vector2(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y)
        );
    }

    private void CreateBackground()
    {
        GameObject background = GetOrCreateChild("Generated Background");
        background.transform.localPosition = Vector3.zero;

        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = background.AddComponent<SpriteRenderer>();
        }

        spriteRenderer.sprite = backgroundSpriteAsset;
        spriteRenderer.drawMode = SpriteDrawMode.Simple;
        spriteRenderer.sortingOrder = backgroundSortingOrder;

        if (spriteRenderer.sprite == null)
        {
            return;
        }

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        background.transform.localScale = new Vector3(
            mapSize.x / spriteSize.x,
            mapSize.y / spriteSize.y,
            1f
        );
    }

    private void CreateBoundary()
    {
        CreateWall("Top Wall", new Vector2(0f, mapSize.y * 0.5f + wallThickness * 0.5f), new Vector2(mapSize.x + wallThickness * 2f, wallThickness));
        CreateWall("Bottom Wall", new Vector2(0f, -mapSize.y * 0.5f - wallThickness * 0.5f), new Vector2(mapSize.x + wallThickness * 2f, wallThickness));
        CreateWall("Left Wall", new Vector2(-mapSize.x * 0.5f - wallThickness * 0.5f, 0f), new Vector2(wallThickness, mapSize.y + wallThickness * 2f));
        CreateWall("Right Wall", new Vector2(mapSize.x * 0.5f + wallThickness * 0.5f, 0f), new Vector2(wallThickness, mapSize.y + wallThickness * 2f));
    }

    private void CreateWall(string wallName, Vector2 localPosition, Vector2 size)
    {
        GameObject wall = GetOrCreateChild(wallName);
        wall.transform.localPosition = localPosition;

        BoxCollider2D collider = wall.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = wall.AddComponent<BoxCollider2D>();
        }

        collider.size = size;
        collider.isTrigger = false;
    }

    private GameObject GetOrCreateChild(string childName)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {
            return child.gameObject;
        }

        GameObject childObject = new GameObject(childName);
        childObject.transform.SetParent(transform);
        childObject.transform.localRotation = Quaternion.identity;
        childObject.transform.localScale = Vector3.one;
        return childObject;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, mapSize);
    }
}
