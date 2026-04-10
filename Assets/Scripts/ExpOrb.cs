using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    [SerializeField] private int expValue = 1;
    [SerializeField] private float followSpeed = 6f;

    private Transform target;
    private bool isFollowing = false;

    public void StartFollowing(Transform player)
    {
        target = player;
        isFollowing = true;
    }

    private void Update()
    {
        if (isFollowing && target != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                followSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerExperience playerExp = other.GetComponent<PlayerExperience>();
            if (playerExp != null)
            {
                playerExp.AddExperience(expValue);
            }

            Destroy(gameObject);
        }
    }
}