using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TradeNode : MonoBehaviour
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

    [Serializable]
    public class NeededResource
    {
        public ResourceType type;
        public int amount;
        public float maxBuyingPrice;
        public float askingPrice;
    }
    public List<NeededResource> NeededResources;

    public List<TradeRoute> TradeRoutes;

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

    public string GetResourcesNeededAsString()
    {
        string s = "";
        foreach (NeededResource resource in NeededResources)
        {
            s += resource.type.ToString() + " (" + resource.amount + ") $" + resource.maxBuyingPrice + " $" + resource.askingPrice + "\n";
        }
        s.TrimEnd('\n');
        return s;
    }

    public TradeNode GetOther(TradeRoute tradeRoute)
    {
        if (tradeRoute.Node1 == this)
        {
            return tradeRoute.Node2;
        }
        if (tradeRoute.Node2 == this)
        {
            return tradeRoute.Node1;
        }
        throw new Exception("Node not in trade route.");
    }

    public TradeRoute GetTradeRoute(TradeNode other)
    {
        foreach (TradeRoute tradeRoute in TradeRoutes)
        {
            if (GetOther(tradeRoute) == other)
            {
                return tradeRoute;
            }
        }
        return null;
    }

    public bool IsProducingResource(ResourceType resourceType)
    {
        return ProducedResources.Where(t => t.type == resourceType).ToList().Count > 0;
    }

    public bool DoesNeedResource(ResourceType resourceType)
    {
        return NeededResources.Where(t => t.type == resourceType).ToList().Count > 0;
    }

    public ProducedResource GetProducedResourceOfType(ResourceType resourceType)
    {
        foreach (ProducedResource resource in ProducedResources)
        {
            if (resource.type == resourceType) return resource;
        }
        return null;
    }

    public NeededResource GetNeededResourceOfType(ResourceType resourceType)
    {
        foreach (NeededResource resource in NeededResources)
        {
            if (resource.type == resourceType) return resource;
        }
        return null;
    }

    public void ChangeNeededAskingPrice(ResourceType resourceType, float priceChange)
    {
        NeededResource resource = GetNeededResourceOfType(resourceType);
        float newPrice = resource.askingPrice + priceChange;
        resource.askingPrice = Mathf.Clamp(newPrice, 0, resource.maxBuyingPrice);
    }
    public void ChangeProducedAskingPrice(ResourceType resourceType, float priceChange)
    {
        ProducedResource resource = GetProducedResourceOfType(resourceType);
        float newPrice = resource.askingPrice + priceChange;
        resource.askingPrice = Mathf.Clamp(newPrice, resource.minSellingPrice, float.MaxValue);
    }

}
