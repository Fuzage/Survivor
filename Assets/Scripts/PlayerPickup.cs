using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private Transform player;

    private PlayerStats stats;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        if (player != null)
        {
            stats = player.GetComponent<PlayerStats>();
            if (stats == null)
            {
                stats = player.gameObject.AddComponent<PlayerStats>();
            }
        }
        else
        {
            stats = GetComponent<PlayerStats>();
            if (stats == null)
            {
                stats = gameObject.AddComponent<PlayerStats>();
            }
        }

        circleCollider = GetComponent<CircleCollider2D>();
        UpdatePickupRadius();
    }

    public void SetPickupRadius(float newRadius)
    {
        if (stats != null)
        {
            stats.pickupRange = newRadius;
        }

        UpdatePickupRadius();
    }

    public void AddPickupRadius(float amount)
    {
        if (stats != null)
        {
            stats.pickupRange += amount;
        }

        UpdatePickupRadius();
    }

    private void UpdatePickupRadius()
    {
        if (circleCollider != null)
        {
            circleCollider.radius = GetPickupRange();
        }
    }

    private float GetPickupRange()
    {
        return stats != null ? stats.pickupRange : 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ExpOrb"))
        {
            ExpOrb orb = other.GetComponent<ExpOrb>();
            if (orb != null)
            {
                orb.StartFollowing(player);
            }
        }
    }
}
