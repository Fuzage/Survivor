using Unity.VisualScripting;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float pickupRadius = 1.5f;

    private CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        UpdatePickupRadius();
    }

    public void SetPickupRadius(float newRadius)
    {
        pickupRadius = newRadius;
        UpdatePickupRadius();
    }

    public void AddPickupRadius(float amount)
    {
        pickupRadius += amount;
        UpdatePickupRadius();
    }

    private void UpdatePickupRadius()
    {
        if (circleCollider != null)
        {
            circleCollider.radius = pickupRadius;
        }
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