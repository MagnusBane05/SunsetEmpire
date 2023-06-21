using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Node : MonoBehaviour
{
	static int nextId;
	public int Id { get; private set; }

	public List<Edge> Edges { get; private set; }
	public int Cost { get; set; }
	public bool Visited { get; set; }
	public bool OnPath { get; set; }
	public Node Previous { get; set; }

	public Node()
	{
		Id = Interlocked.Increment(ref nextId);
		Edges = new();
	}

	public override bool Equals(object obj) => Equals(obj as Node);

	public bool Equals(Node other)
	{
		if (other is null)
		{
			return false;
		}

		if (GetType() != other.GetType())
		{
			return false;
		}

		return (Id == other.Id);
	}
	public override int GetHashCode() => Id.GetHashCode();

	public Node GetOtherNode(Edge e)
	{
		if (e.Node1.Id == Id)
		{
			return e.Node2;
		}
		if (e.Node2.Id == Id)
		{
			return e.Node1;
		}
		return this;
	}

	public void AddEdge(Edge edge)
	{
		Edges.Add(edge);
	}
}

public class Edge
{
	public Node Node1 { get; private set; }
	public Node Node2 { get; private set; }
	public int Cost { get; set; }

	public Edge(Node node1, Node node2, int cost)
	{
		Node1 = node1;
		Node2 = node2;
		Cost = cost;
	}
}

public class QNode
{
	public Node Node { get; private set; }
	public int Cost { get; private set; }

	public QNode(Node node, int cost)
	{
		Node = node;
		Cost = cost;
	}

}
