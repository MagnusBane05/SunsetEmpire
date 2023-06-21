using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceProducer : MonoBehaviour
{


    [Serializable]
    public class ProducedResource
    {
        public ResourceType type;
        public int amount;
        public float minSellingPrice;
        public float askingPrice;
    }
    public List<ProducedResource> ProducedResources;

    public string GetResourcesProducedAsString()
    {
        string s = "";
        foreach (ProducedResource resource in ProducedResources)
        {
            s += resource.type.ToString() + " (" + resource.amount + ") $" + resource.minSellingPrice + " $" + resource.askingPrice + "\n";
        }
        s.TrimEnd('\n');
        return s;
    }

    public bool IsProducingResource(ResourceType resourceType)
    {
        return ProducedResources.Where(t => t.type == resourceType).ToList().Count > 0;
    }

    public ProducedResource GetProducedResourceOfType(ResourceType resourceType)
    {
        foreach (ProducedResource resource in ProducedResources)
        {
            if (resource.type == resourceType) return resource;
        }
        return null;
    }
    public void ChangeProducedAskingPrice(ResourceType resourceType, float priceChange)
    {
        ProducedResource resource = GetProducedResourceOfType(resourceType);
        float newPrice = resource.askingPrice + priceChange;
        resource.askingPrice = Mathf.Clamp(newPrice, resource.minSellingPrice, float.MaxValue);
    }

}
