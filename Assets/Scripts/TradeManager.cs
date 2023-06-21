using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wheat,
    Iron,
    Fish
}
public readonly struct Trade
{

    public Trade(TradeNode origin, TradeNode destination, ResourceType resourceType, int amount, float price)
    {
        Origin = origin;
        Destination = destination;
        ResourceType = resourceType;
        Amount = amount;
        Price = price;
    }

    public TradeNode Origin { get; }
    public TradeNode Destination { get; }
    public ResourceType ResourceType { get; }
    public int Amount { get; }
    public float Price { get; }
}

public class TradeManager : MonoBehaviour
{

    public static TradeManager i;

    private List<Trade> trades;

    // Prefabs
    public GameObject CaravanPrefab;

    // Materials
    public Material WheatMaterial;
    public Material FishMaterial;
    public Material IronMaterial;

    // Trade cooldowns
    public int WheatCooldown = 2;
    public int FishCooldown = 1;
    public int IronCooldown = 6;


    private void Start()
    {
        i = this;
        trades = new();
    }

    public void AddTrade(TradeNode origin, TradeNode destination, ResourceType resourceType, int amount, float price)
    {
        Trade trade = new(origin, destination, resourceType, amount, price);
        trades.Add(trade);

        CreateNewCaravan(trade);
    }
    private void CreateNewCaravan(Trade trade)
    {
        GameObject caravanObject = Instantiate(CaravanPrefab, trade.Origin.transform.position, CaravanPrefab.transform.rotation);
        MeshRenderer meshRenderer = caravanObject.GetComponent<MeshRenderer>();
        meshRenderer.material = GetMaterial(trade.ResourceType);

        FollowPath followPathComponent = caravanObject.GetComponent<FollowPath>();
        followPathComponent.NewPath(TradeNodeManager.i.Pathfind(trade.Origin, trade.Destination));

        CaravanText caravanTextComponent = caravanObject.GetComponent<CaravanText>();
        caravanTextComponent.Trade = trade;
    }

    public Material GetMaterial(ResourceType resourceType)
    {
        return resourceType switch
        {
            ResourceType.Wheat => WheatMaterial,
            ResourceType.Fish => FishMaterial,
            ResourceType.Iron => IronMaterial,
            _ => WheatMaterial,
        };
    }

    public int GetCooldown(ResourceType resourceType)
    {
        return resourceType switch
        {
            ResourceType.Wheat => WheatCooldown,
            ResourceType.Fish => FishCooldown,
            ResourceType.Iron => IronCooldown,
            _ => WheatCooldown,
        };
    }
}
