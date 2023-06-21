using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Buyer
{
    public TradeNode TradeNode;
    public int GoodsToBuy;
    public List<Seller> Sellers;
}

public class Seller
{
    public TradeNode TradeNode;
    public int AvailableGoods;
}

public class TradeSimulator : MonoBehaviour
{
    private List<Buyer> buyers;
    private List<Seller> sellers;
    private ResourceType resourceType;

    public void StartNewTrade(ResourceType resourceType, List<TradeNode> buyers, List<TradeNode> sellers)
    {
        this.buyers = new();
        this.sellers = new();
        this.resourceType = resourceType;
        foreach (TradeNode seller in sellers)
        {
            Seller newSeller = new();
            newSeller.TradeNode = seller;
            newSeller.AvailableGoods = seller.GetProducedResourceOfType(resourceType).amount;
            this.sellers.Add(newSeller);
        }
        foreach (TradeNode buyer in buyers)
        {
            Buyer newBuyer = new();
            newBuyer.TradeNode = buyer;
            newBuyer.GoodsToBuy = buyer.GetNeededResourceOfType(resourceType).amount;
            newBuyer.Sellers = GetSellersForBuyer(newBuyer, this.sellers);
            this.buyers.Add(newBuyer);
        }

        Trade();
    }

    private List<Seller> GetSellersForBuyer(Buyer buyer, List<Seller> sellers)
    {
        return sellers.Where(t => TradeNodeManager.i.Pathfind(buyer.TradeNode, t.TradeNode).Count > 0 && AgreeablePurchase(CalculateBuyerAskingPrice(buyer), CalculateSellerAskingPrice(t))).ToList();
    }

    private void Trade()
    {
        while (DoesTradeContinue())
        {
            foreach (Buyer buyer in buyers)
            {
                FindTradeForBuyer(buyer);
            }
        }

        UpdateAskingPrices();
    }

    private void UpdateAskingPrices()
    {
        buyers.ForEach(UpdateBuyerAskingPrice);
        sellers.ForEach(UpdateSellerAskingPrice);
    }

    private void UpdateSellerAskingPrice(Seller seller) =>
        seller.TradeNode.ChangeProducedAskingPrice(resourceType, seller.AvailableGoods == 0 ? 1 : -1);

    private void UpdateBuyerAskingPrice(Buyer buyer) =>
        buyer.TradeNode.ChangeNeededAskingPrice(resourceType, buyer.GoodsToBuy == 0 ? -1 : 1);

    private void FindTradeForBuyer(Buyer buyer)
    {
        if (buyer.Sellers.Count == 0) return;

        int index = UnityEngine.Random.Range(0, buyer.Sellers.Count);
        Seller seller = buyer.Sellers[index];

        int goodsToBuy = Math.Min(buyer.GoodsToBuy, seller.AvailableGoods);

        if (goodsToBuy == 0) return;

        float buyerAskingPrice = CalculateBuyerAskingPrice(buyer);
        float sellerAskingPrice = CalculateSellerAskingPrice(seller);

        if (!AgreeablePurchase(buyerAskingPrice, sellerAskingPrice))
        {
            buyer.Sellers.Remove(seller);
            return;
        }

        ManageSuccessfulTrade(buyer, seller, goodsToBuy, sellerAskingPrice);

    }

    private void ManageSuccessfulTrade(Buyer buyer, Seller seller, int goodsToBuy, float sellerAskingPrice)
    {
        buyer.GoodsToBuy -= goodsToBuy;
        seller.AvailableGoods -= goodsToBuy;
        buyer.Sellers.Remove(seller);

        // Debug.Log(seller.TradeNode.name + " sold " + goodsToBuy + " " + resourceType.ToString() + " to " + buyer.TradeNode.name + " for $" + sellerAskingPrice);

        TradeManager.i.AddTrade(seller.TradeNode, buyer.TradeNode, resourceType, goodsToBuy, sellerAskingPrice);

    }

    private bool AgreeablePurchase(float buyerAskingPrice, float sellerAskingPrice)
    {
        return sellerAskingPrice <= buyerAskingPrice;
    }

    private float CalculateBuyerAskingPrice(Buyer buyer)
    {
        return buyer.TradeNode.GetNeededResourceOfType(resourceType).askingPrice;
    }
    private float CalculateSellerAskingPrice(Seller seller)
    {
        return seller.TradeNode.GetProducedResourceOfType(resourceType).askingPrice;
    }

    private bool DoesTradeContinue()
    {
        foreach (Buyer buyer in buyers)
        {
            if (buyer.GoodsToBuy == 0) continue;
            if (buyer.Sellers.Count > 0) return true;
        }
        return false;
    }

}
