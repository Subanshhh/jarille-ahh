using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D fieldCollider;

    [Header("Camera Padding Outside Field")]
    public float horizontalPadding = 2f;
    public float verticalPadding = 2f;

    private Camera cam;
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        Vector3 targetPos = target.position;
        Bounds bounds = fieldCollider.bounds;

        float minX = bounds.min.x - horizontalPadding + halfWidth;
        float maxX = bounds.max.x + horizontalPadding - halfWidth;

        float minY = bounds.min.y - verticalPadding + halfHeight;
        float maxY = bounds.max.y + verticalPadding - halfHeight;

        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}