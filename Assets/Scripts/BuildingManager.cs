using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class BuildingData
{
    public Vector3 position;
    public int prefabIndex; // Identify which prefab was used
    public bool isDefault;
    public string id;
}

[System.Serializable]
public class BuildingListWrapper
{
    public List<BuildingData> buildings;
}


public class BuildingManager : MonoBehaviour
{
    public List<GameObject> buildingPrefabs; // List of building prefabs
    private List<GameObject> buildings = new List<GameObject>();
    private string saveFilePath;

    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/buildingData.json";

        // Find all default buildings in the scene
        GameObject[] defaultBuildings = GameObject.FindGameObjectsWithTag("DefaultBuilding");
        foreach (GameObject building in defaultBuildings)
        {
            buildings.Add(building);
        }

        // Load building data
        LoadBuildings();
    }

    public void AddBuilding(Vector3 position, int prefabIndex, bool isDefault = false)
    {
        GameObject newBuilding;

        if (isDefault)
        {
            // Use the existing default building GameObject
            newBuilding = buildings.Find(b => b.transform.position == position);
        }
        else
        {
            // Instantiate a new building based on the prefab index
            newBuilding = Instantiate(buildingPrefabs[prefabIndex], position, Quaternion.identity);
            buildings.Add(newBuilding);
        }
    }

    public void SaveBuildings()
    {
        List<BuildingData> buildingDataList = new List<BuildingData>();
        foreach (GameObject building in buildings)
        {
            BuildingType type = building.GetComponent<BuildingType>();
            BuildingData data = new BuildingData
            {
                position = building.transform.position,
                prefabIndex = buildingPrefabs.IndexOf(building.GetComponent<BuildingType>().buildingPrefab),
                isDefault = building.CompareTag("DefaultBuilding"),
                id = type.id
            };
            buildingDataList.Add(data);
        }
        
        BuildingListWrapper wrapper = new BuildingListWrapper
        {
            buildings = buildingDataList
        };

        string json = JsonUtility.ToJson(wrapper, true);
        
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadBuildings()
    {
        if (!File.Exists(saveFilePath)) return;
        string json = File.ReadAllText(saveFilePath);
        if (json == "") {
            return;
        }

        var buildingDataList = JsonUtility.FromJson<BuildingList>(json);

        foreach (BuildingData data in buildingDataList.buildings)
        {
            if (data.isDefault)
            {
                GameObject defaultBuilding = buildings.Find(b => b.GetComponent<BuildingType>().id == data.id);
                if (defaultBuilding)
                {
                    defaultBuilding.transform.position = data.position;
                }
            }
            else
            {
                AddBuilding(data.position, data.prefabIndex, false);
            }
        }
    }
}

[System.Serializable]
public class BuildingList
{
    public List<BuildingData> buildings;
}
