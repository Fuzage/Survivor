using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.08f;
    [SerializeField] private bool clampToMapArea = true;

    private Camera cameraComponent;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        if (clampToMapArea && MapArea.Instance != null && cameraComponent != null && cameraComponent.orthographic)
        {
            float cameraHalfHeight = cameraComponent.orthographicSize;
            float cameraHalfWidth = cameraHalfHeight * cameraComponent.aspect;

            Vector2 mapMin = MapArea.Instance.Min;
            Vector2 mapMax = MapArea.Instance.Max;

            targetPosition.x = Mathf.Clamp(targetPosition.x, mapMin.x + cameraHalfWidth, mapMax.x - cameraHalfWidth);
            targetPosition.y = Mathf.Clamp(targetPosition.y, mapMin.y + cameraHalfHeight, mapMax.y - cameraHalfHeight);
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}
