using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset; // To store the offset between the object and mouse position
    private BuildingManager buildingManager;

    void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();
    }

    void OnMouseDown()
    {
        isDragging = true;

        // Calculate the offset between the object's position and the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    void OnMouseUp()
    {
        isDragging = false;
        buildingManager.SaveBuildings(); // Save the new position of all buildings
    }

    void Update()
    {
        if (isDragging)
        {
            // Update the position with the calculated offset
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z) + offset;
        }
    }
}
