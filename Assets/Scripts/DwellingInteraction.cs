using UnityEngine;

public class DwellingInteraction : MonoBehaviour
{
    private ResourceManager resourceManager;

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();

        if (resourceManager != null)
        {
            resourceManager.AddDwelling(gameObject); // Register Dwelling with ResourceManager
        }
        else
        {
            Debug.LogError("ResourceManager not found in the Scene.");
        }
    }
}
