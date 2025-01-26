using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 10f; // Speed of horizontal movement
    public Vector2 scrollLimits = new Vector2(-10f, 10f); // X-axis boundaries for camera movement

    void Update()
    {
        // Horizontal movement with keys
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 newPosition = transform.position + new Vector3(horizontalInput * scrollSpeed * Time.deltaTime, 0, 0);

        // Mouse-driven movement (move when mouse is near the screen edges)
        if (Input.mousePosition.x < 50) // Left edge
            newPosition.x -= scrollSpeed * Time.deltaTime;
        if (Input.mousePosition.x > Screen.width - 50) // Right edge
            newPosition.x += scrollSpeed * Time.deltaTime;

        // Clamp position
        newPosition.x = Mathf.Clamp(newPosition.x, scrollLimits.x, scrollLimits.y);

        // Apply position
        transform.position = newPosition;
    }

}
