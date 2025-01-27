using System;

[Serializable]
public class Resource
{
    public string name; // Name of the resource (e.g., "Qi", "Population")
    public float quantity; // Current amount of the resource

    public Resource(string name, float initialQuantity)
    {
        this.name = name;
        this.quantity = initialQuantity;
    }
}
