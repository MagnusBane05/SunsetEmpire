using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TradeNodeManager : MonoBehaviour
{

    public static TradeNodeManager i;

    public List<TradeNode> TradeNodes;
    private TradeSimulator tradeSimulator;
    private float t = 0;

    private Graph graph;

    // Start is called before the first frame update
    void Start()
    {
        tradeSimulator = GetComponent<TradeSimulator>();
        graph = GetComponent<Graph>();
        i = this;
        InitializeGraphNodes();
    }

    private void InitializeGraphNodes()
    {
        graph.Nodes = new();
        foreach (TradeNode tradeNode in TradeNodes)
        {
            InitializeGraphNode(tradeNode);
        }
    }

    private void InitializeGraphNode(TradeNode tradeNode)
    {
        Node node = tradeNode.GetComponent<Node>();

        if (!node)
        {
            Debug.LogError(tradeNode + " does not have a Node component.");
            return;
        }

        foreach (TradeRoute tradeRoute in tradeNode.TradeRoutes)
        {
            int cost = tradeRoute.GetCost();
            TradeNode otherTradeNode = tradeNode.GetOther(tradeRoute);
            Node otherNode = otherTradeNode.GetComponent<Node>();

            Edge edge = new Edge(node, otherNode, cost);
            node.AddEdge(edge);
        }

        graph.Nodes.Add(node);

    }



    public List<TradeNode> Pathfind(TradeNode startNode, TradeNode endNode)
    {
        Node startGraphNode = startNode.GetComponent<Node>();
        Node endGraphNode = endNode.GetComponent<Node>();

        if (!startGraphNode || !endGraphNode)
        {
            Debug.LogError("Start or end TradeNode does not have a Node component.");
            return null;
        }

        List<Node> nodePath = graph.PathFind(startGraphNode, endGraphNode);

        List<TradeNode> path = new();

        foreach (Node node in nodePath)
        {
            TradeNode tradeNode = node.GetComponent<TradeNode>();
            path.Add(tradeNode);
        }

        return path;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        foreach (ResourceType resourceType in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
        {
            TradeResource(resourceType);
        }
        
    }

    private void TradeResource(ResourceType resourceType)
    {
        int resourceCooldown = TradeManager.i.GetCooldown(resourceType);        
        if (!((int)t / resourceCooldown > (int)(t - Time.deltaTime) / resourceCooldown)) return;

        List<TradeNode> buyers = CollectBuyers(resourceType);
        List<TradeNode> sellers = CollectSellers(resourceType);
        PerformTrade(resourceType, buyers, sellers);
    }

    private void PerformTrade(ResourceType resourceType, List<TradeNode> buyers, List<TradeNode> sellers)
    {
        tradeSimulator.StartNewTrade(resourceType, buyers, sellers);
    }

    private List<TradeNode> CollectBuyers(ResourceType resourceType) =>
        TradeNodes.Where(t => t.DoesNeedResource(resourceType)).ToList();

    private List<TradeNode> CollectSellers(ResourceType resourceType) =>
        TradeNodes.Where(t => t.IsProducingResource(resourceType)).ToList();

}
