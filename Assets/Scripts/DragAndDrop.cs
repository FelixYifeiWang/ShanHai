using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset; // To store the offset between the object and mouse position
    private BuildingManager buildingManager;
    private ResourceManager resourceManager;

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        buildingManager = FindObjectOfType<BuildingManager>();
        Debug.Log(resourceManager.dragAndDropEnabled);
    }

    void OnMouseDown()
    {
        if (resourceManager.dragAndDropEnabled)
        {
            isDragging = true;
            // Calculate the offset between the object's position and the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = transform.position - new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }

    void OnMouseUp()
    {
        if (isDragging) {
            isDragging = false;
            buildingManager.SaveBuildings(); // Save the new position of all buildings
        }
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
