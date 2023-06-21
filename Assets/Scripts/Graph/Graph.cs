using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
	public List<Node> Nodes { get; set; }

	public List<Node> PathFind(Node startNode, Node endNode)
	{
		PriorityQueue<QNode, int> priorityQueue = new();
		ClearNodes();

		QNode startQNode = new(startNode, 0);
		priorityQueue.Enqueue(startQNode, startQNode.Cost);

		while (priorityQueue.Count > 0)
		{

			QNode lowest = priorityQueue.Dequeue();
			if (lowest.Node.Equals(endNode))
				break;

			OpenLowestNode(endNode, priorityQueue, lowest);

		}

		List<Node> pathNodes = new();
		AddNodesToPath(startNode, endNode, pathNodes);

		return pathNodes;
	}
	
	private void AddNodesToPath(Node startNode, Node endNode, List<Node> pathNodes)
	{
		Node currentNode = endNode.Previous;
		if (currentNode == null)
			return;

		pathNodes.Add(endNode);
		while (!currentNode.Equals(startNode))
		{
			pathNodes.Add(currentNode);
			currentNode.OnPath = true;
			currentNode = currentNode.Previous;
		}
		pathNodes.Add(currentNode);
		pathNodes.Reverse();
	}

	private void ClearNodes()
	{
		foreach (Node node in Nodes)
		{
			node.Cost = int.MaxValue;
			node.OnPath = false;
			node.Visited = false;
			node.Previous = null;
		}
	}
	
	private void OpenLowestNode(Node endNode, PriorityQueue<QNode, int> priorityQueue, QNode lowest)
	{
		lowest.Node.Visited = true;
		foreach (Edge neighbour in lowest.Node.Edges)
		{
			ProcessNeighbours(endNode, priorityQueue, lowest, neighbour);
		}
	}

	private void ProcessNeighbours(Node endNode, PriorityQueue<QNode, int> priorityQueue, QNode lowest, Edge neighbour)
	{
		Node node = lowest.Node.GetOtherNode(neighbour);

		// don't add node if friendly unit on it

		int nodeCost = lowest.Cost + neighbour.Cost;
		if (node.Cost <= nodeCost) return;

		node.Cost = nodeCost;
		node.Previous = lowest.Node;

		// estimate distance
		float mDist = Vector3.Distance(node.transform.position, endNode.transform.position);
		float estimate = mDist * neighbour.Cost;

		QNode updatedNode = new(node, nodeCost + (int)estimate);
		priorityQueue.Enqueue(updatedNode, updatedNode.Cost);
	}
}
