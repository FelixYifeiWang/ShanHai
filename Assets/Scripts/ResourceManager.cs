using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceManager : MonoBehaviour
{
    public List<Resource> resources = new List<Resource>();
    private string saveFilePath;
    public bool dragAndDropEnabled = false;

    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/resources.json";

        // Initialize resources or load saved data
        if (File.Exists(saveFilePath))
        {
            LoadResources();
        }
        else
        {
            // Initialize resources with default values
            AddResource("Qi", 100);
            AddResource("Population", 2);
            AddResource("Food", 200);
            AddResource("Space", 10);
            AddResource("Money", 0);
            AddResource("Culture", 0);
            AddResource("ActPoint", 2); 
            AddResource("Turn Number", 1);
        }
        StartCoroutine(AutoSave());
    }

    // Add a new resource
    public void AddResource(string name, float initialQuantity)
    {
        Resource resource = new Resource(name, initialQuantity);
        resources.Add(resource);
    }

    // Update the quantity of a resource
    public void UpdateResource(string name, float amount)
    {
        Resource resource = resources.Find(r => r.name == name);
        if (resource != null)
        {
            resource.quantity += amount;
            resource.quantity = Mathf.Max(0, resource.quantity); // Prevent negative values
            SaveResources();
        }
        else
        {
            Debug.LogWarning($"Resource '{name}' not found.");
        }
    }

    // Get the current quantity of a resource
    public float GetResourceQuantity(string name)
    {
        Resource resource = resources.Find(r => r.name == name);
        return resource != null ? resource.quantity : 0;
    }

    // Save the resources to a JSON file
    public void SaveResources()
    {
        string json = JsonUtility.ToJson(new ResourceListWrapper(resources), true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Resources saved to " + saveFilePath);
    }

    // Load resources from a JSON file
    public void LoadResources()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            ResourceListWrapper wrapper = JsonUtility.FromJson<ResourceListWrapper>(json);
            resources = wrapper.resources;
            Debug.Log("Resources loaded from " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("Save file not found. Initializing default resources.");
        }
    }

    void OnApplicationQuit()
    {
        SaveResources();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            Debug.Log("Autosaving resources...");
            yield return new WaitForSeconds(60f); // Wait for 60 seconds
        }
    }

    public void EndTurn()
    {
        // Increment Turn Number
        Debug.Log(GetResourceQuantity("Turn Number"));
        UpdateResource("Turn Number", 1);
        Debug.Log(GetResourceQuantity("Turn Number"));

        // Set ActPoint to the value of Population
        float population = GetResourceQuantity("Population");
        Resource actPoint = resources.Find(r => r.name == "ActPoint");
        if (actPoint != null)
        {
            actPoint.quantity = population;
        }

        Debug.Log("Turn Ended. New Turn Number: " + GetResourceQuantity("Turn Number"));
    }

}

// Wrapper class for serialization
[System.Serializable]
public class ResourceListWrapper
{
    public List<Resource> resources;

    public ResourceListWrapper(List<Resource> resources)
    {
        this.resources = resources;
    }
}

