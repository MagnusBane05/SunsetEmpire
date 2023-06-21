using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{

    private List<BezierSpline> splinePath;
    private List<float> lengths; // lengths[i] is the length of curve segments from 0 to the start of curve i
    private float t;
    public float Speed = 1f;
    public event Action DestinationReached;

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (splinePath.Count == 0) return;

        t += Time.deltaTime * Speed;

        transform.position = Interpolate(t);

    }

    private Vector3 Interpolate(float t)
    {
        int curve = FindCurrentCurveSegment();

        if (curve == lengths.Count - 1)
        {
            Vector3 endPoint = splinePath[splinePath.Count - 1].P3.position;
            OnDestinationReached();
            return endPoint;
        }

        float s = (t - lengths[curve]) / (lengths[curve + 1] - lengths[curve]);
        return splinePath[curve].Interpolate(s);
    }

    private int FindCurrentCurveSegment()
    {
        int curve = 0;
        for (int i = 0; (i < lengths.Count) && (t > lengths[i]); i++)
        {
            curve = i;
        }

        return curve;
    }

    private void OnDestinationReached()
    {
        Debug.Log("Reached destination");
        DestinationReached?.Invoke();
        Destroy(gameObject);
    }

    public void NewPath(List<TradeNode> tradeNodes)
    {
        splinePath = new();
        lengths = new();
        for (int i = 1; i < tradeNodes.Count; i++)
        {
            TradeNode origin = tradeNodes[i - 1];
            TradeNode destination = tradeNodes[i];
            TradeRoute tradeRoute = origin.GetTradeRoute(destination);

            AddSplineToPath(origin.transform, new(tradeRoute.BezierSpline));
        }
        
        lengths.Add(0f);
        for (int i = 0; i < splinePath.Count; i++)
        {
            lengths.Add(lengths[i] + splinePath[i].EstimateLength());
        }
    }

    private void AddSplineToPath(Transform origin, BezierSpline bezierSpline)
    {
        if (bezierSpline.P0 == origin)
        {
            splinePath.Add(bezierSpline);
            return;
        }

        BezierSpline reverseSpline = new();
        reverseSpline.P0 = bezierSpline.P3;
        reverseSpline.P1 = bezierSpline.P2;
        reverseSpline.P2 = bezierSpline.P1;
        reverseSpline.P3 = bezierSpline.P0;

        splinePath.Add(reverseSpline);
    }
}
