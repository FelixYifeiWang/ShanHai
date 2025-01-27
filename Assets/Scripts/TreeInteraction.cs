using UnityEngine;

public class TreeInteraction : MonoBehaviour
{
    private ResourceManager resourceManager;

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();

        if (resourceManager == null)
        {
            Debug.LogError("ResourceManager not found in the Scene. Make sure it exists and is active.");
        }
    }

    void OnMouseDown()
    {
        // Check if drag-and-drop is disabled and ActPoint > 0
        if (resourceManager != null && !resourceManager.dragAndDropEnabled)
        {
            float actPoint = resourceManager.GetResourceQuantity("ActPoint");
            if (actPoint > 0)
            {
                // Deduct 1 ActPoint and add 100 Money
                resourceManager.UpdateResource("ActPoint", -1);
                resourceManager.UpdateResource("Money", 100);

                Debug.Log("Tree clicked: ActPoint - 1, Money + 100");
            }
            else
            {
                Debug.Log("Not enough ActPoint to interact with the Tree.");
            }
        }
    }
}
