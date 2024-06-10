using UnityEngine;

public class CameraController : MonoBehaviour
{
    //target of the camera
    [Header("Camera Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float padding;

    [Header("Level Boundaries")]
    // References to the boundary objects
    [SerializeField] private Transform leftBound;
    [SerializeField] private Transform rightBound;
    [SerializeField] private Transform bottomBound;

    private Vector3 targetPoint = Vector3.zero; // empty vector to store target point of camera
    private float cameraHalfWidth; // for use in making camera edges stop at level boundaries
    private float cameraHalfHeight;// ^
    private Rigidbody2D playerBody;

    private void Start()
    {
        // Calculate the half width and height of the camera view
        cameraHalfHeight = Camera.main.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

        // Camera starts on player, putting player into the serialized transform field allows camera to follow
        targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

        playerBody = player.GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        // movement of camera to follow player
        targetPoint.x = player.transform.position.x;
        targetPoint.y = player.transform.position.y;
        
        // Get the boundary positions
        float minX = leftBound.position.x + cameraHalfWidth;
        float maxX = rightBound.position.x - cameraHalfWidth;
        float minY = bottomBound.position.y + cameraHalfHeight;

        // Clamp the camera position to the bounds of the level
        targetPoint.x = Mathf.Clamp(targetPoint.x, minX, maxX);
        targetPoint.y = Mathf.Clamp(targetPoint.y, minY, float.MaxValue); // Assuming no upper bound on Y

        // Set the camera position
        transform.position = new Vector3(targetPoint.x, targetPoint.y, transform.position.z);
    }
}
