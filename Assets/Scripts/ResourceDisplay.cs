using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    private ResourceManager resourceManager;

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
    }

    void OnGUI()
    {
        if (resourceManager == null) return;

        // Display Turn Number in the top-right corner
        float turnNumber = resourceManager.GetResourceQuantity("Turn Number");
        GUI.Label(new Rect(Screen.width - 350, 10, 140, 40), $"Turn: {turnNumber}", new GUIStyle()
        {
            fontSize = 90,
            normal = new GUIStyleState() { textColor = Color.black }
        });

        // Display other resources in the top-left corner
        string resourceText = "Resources:\n";
        foreach (var resource in resourceManager.resources)
        {
            if (resource.name != "Turn Number" && resource.name != "ActPoint")
            {
                resourceText += $"{resource.name}: {resource.quantity}\n";
            }
        }

        GUI.Label(new Rect(10, 10, 300, 300), resourceText, new GUIStyle()
        {
            fontSize = 50,
            normal = new GUIStyleState() { textColor = Color.black }
        });

        float actPoint = resourceManager.GetResourceQuantity("ActPoint");
        GUI.Label(new Rect(Screen.width - 450, Screen.height - 60, 140, 50), $"ActPoint: {actPoint}", new GUIStyle()
        {
            fontSize = 50,
            normal = new GUIStyleState() { textColor = Color.black }
        });


        if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 130, 140, 50), 
                       resourceManager.dragAndDropEnabled ? "Disable Drag" : "Enable Drag"))
        {
            resourceManager.dragAndDropEnabled = !resourceManager.dragAndDropEnabled;
            Debug.Log($"Drag-and-Drop Enabled: {resourceManager.dragAndDropEnabled}");
        }

        if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 60, 140, 50), "End Today"))
        {
            resourceManager.EndTurn();
        }
    }
}
