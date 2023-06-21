using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeRoute : MonoBehaviour
{

    public TradeNode Node1;
    public TradeNode Node2;

    private TradeNode m_Node1;
    private TradeNode m_Node2;

    public Transform ControlPoint1;
    public Transform ControlPoint2;

    private Transform m_ControlPoint1;
    private Transform m_ControlPoint2;

    public BezierSplineComponent BezierSpline;

    void Awake()
    {
        BezierSpline = GetComponent<BezierSplineComponent>();
        //BezierSpline = new(Node1.transform, ControlPoint1.transform, ControlPoint2.transform, Node2.transform);
    }

    private void OnValidate()
    {
        BezierSpline = GetComponent<BezierSplineComponent>();
        if (Node1 != m_Node1)
        {
            OnNodeChanged(m_Node1, Node1);
            m_Node1 = Node1;
            UpdateName();
            BezierSpline.SetPoints();
        }

        if (Node2 != m_Node2)
        {
            OnNodeChanged(m_Node2, Node2);
            m_Node2 = Node2;
            UpdateName();
            BezierSpline.SetPoints();
        }

        if (ControlPoint1 != m_ControlPoint1)
        {
            BezierSpline.P1 = ControlPoint1;
            m_ControlPoint1 = ControlPoint1;
            BezierSpline.SetPoints();
        }

        if (ControlPoint2 != m_ControlPoint2)
        {
            BezierSpline.P2 = ControlPoint2;
            m_ControlPoint2 = ControlPoint2;
            BezierSpline.SetPoints();
        }
    }

    private void OnNodeChanged(TradeNode oldNode, TradeNode newNode)
    {
        if (oldNode) oldNode.TradeRoutes.Remove(this);
        newNode.TradeRoutes.Add(this);

        if (!BezierSpline) return;

        if (!oldNode)
        {
            if (BezierSpline.P0 == null) BezierSpline.P0 = newNode.transform;
            else BezierSpline.P3 = newNode.transform;
            return;
        }
        if (BezierSpline.P0 == oldNode.transform) BezierSpline.P0 = newNode.transform;
        if (BezierSpline.P3 == oldNode.transform) BezierSpline.P3 = newNode.transform;
    }

    private void UpdateName()
    {
        if (!Node1 || !Node2)
        {
            name = "Trade route to nowhere";
            return;
        }
        name = Node1.name + " to " + Node2.name;
    }

    public int GetCost()
    {
        return (int) BezierSpline.EstimateLength();
    }
}
