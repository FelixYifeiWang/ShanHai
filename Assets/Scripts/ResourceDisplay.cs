using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    private ResourceManager resourceManager;
    public GameObject dwellingPrefab;
    private BuildingManager buildingManager;

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        buildingManager = FindObjectOfType<BuildingManager>();
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

        if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 180, 140, 50), "Add Dwelling"))
        {
            int dwellingCount = buildingManager.buildings.FindAll(b => b.name == dwellingPrefab.name).Count;
            if (dwellingCount == 0 || 
                (resourceManager.GetResourceQuantity("ActPoint") >= 1 &&
                resourceManager.GetResourceQuantity("Money") >= 100))
            {
                // Deduct 1 ActPoint and 100 Money
                if (dwellingCount > 0)
                {
                    resourceManager.UpdateResource("ActPoint", -1);
                    resourceManager.UpdateResource("Money", -100);
                }

                // Instantiate the Dwelling prefab at a random position
                Vector3 position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);

                int prefabIndex = buildingManager.buildingPrefabs.IndexOf(dwellingPrefab);

                if (prefabIndex >= 0)
                {
                    buildingManager.AddBuilding(position, prefabIndex, false);
                    buildingManager.SaveBuildings();
                }
                else
                {
                    Debug.LogError("Dwelling prefab is not registered in BuildingManager.");
                }

                Debug.Log("Dwelling added! -1 ActPoint, -100 Money");
            }
            else
            {
                Debug.LogWarning("Not enough resources to add a Dwelling.");
            }        }
    }
}
