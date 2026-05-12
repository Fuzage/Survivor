using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float directionUpdateInterval = 0.12f;

    private static Transform cachedPlayer;

    private EnemyStats stats;
    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 moveDirection;
    private float directionUpdateTimer;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        if (stats == null)
        {
            stats = gameObject.AddComponent<EnemyStats>();
        }

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        if (animator != null)
        {
            animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        }
    }

    private void OnEnable()
    {
        EnemyRegistry.Register(this);
    }

    private void OnDisable()
    {
        EnemyRegistry.Unregister(this);
    }

    private void Start()
    {
        if (cachedPlayer == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                cachedPlayer = playerObject.transform;
            }
        }

        if (cachedPlayer != null)
        {
            player = cachedPlayer;
            directionUpdateTimer = Random.Range(0f, directionUpdateInterval);
        }
        else
        {
            Debug.LogWarning("Enemy could not find Player. Make sure the Player object has the 'Player' tag.");
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        directionUpdateTimer -= Time.fixedDeltaTime;
        if (directionUpdateTimer <= 0f)
        {
            moveDirection = ((Vector2)player.position - rb.position).normalized;
            directionUpdateTimer = directionUpdateInterval;
            UpdateFacing(moveDirection);
        }

        rb.linearVelocity = moveDirection * stats.moveSpeed;
    }

    private void UpdateFacing(Vector2 direction)
    {
        if (spriteRenderer == null || Mathf.Approximately(direction.x, 0f)) return;

        spriteRenderer.flipX = direction.x < 0f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(stats.contactDamage);
            }
        }
    }
}
