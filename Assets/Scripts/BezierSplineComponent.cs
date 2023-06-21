using UnityEngine;

public class BezierSpline
{
    public Transform P0;
    public Transform P1;
    public Transform P2;
    public Transform P3;

    public BezierSpline()
    {

    }

    public BezierSpline(Transform p0, Transform p1, Transform p2, Transform p3)
    {
        P0 = p0;
        P1 = p1;
        P2 = p2;
        P3 = p3;
    }

    public BezierSpline(BezierSplineComponent other)
    {
        P0 = other.P0;
        P1 = other.P1;
        P2 = other.P2;
        P3 = other.P3;
    }

    public Vector3 Interpolate(float t)
    {
        Vector3 v1;
        if (P1 == null) v1 = P0.position + Vector3.Normalize(P3.position - P0.position);
        else v1 = P1.position;
        Vector3 v2;
        if (P2 == null) v2 = P3.position + Vector3.Normalize(P0.position - P3.position);
        else v2 = P2.position;

        Vector3 point0 = P0.position;
        Vector3 point1 = v1;
        Vector3 point2 = v2;
        Vector3 point3 = P3.position;

        return (1 - t) * (1 - t) * (1 - t) * point0 + 3 * (1 - t) * (1 - t) *
                t * point1 + 3 * (1 - t) * t * t * point2 + t * t * t * point3;
    }
    public float EstimateLength()
    {
        if (P0 == null || P3 == null)
        {
            Debug.LogError("Cannot estimate length since no start or end position was given");
            return -1;
        }

        int n = 10;
        float lengthEstimate = 0;
        for (int i = 1; i < n; i++)
        {
            Vector3 pointA = Interpolate((i - 1) / (float)n);
            Vector3 pointB = Interpolate(i / (float)n);

            float distanceBetweenPoints = Vector3.Distance(pointA, pointB);
            lengthEstimate += distanceBetweenPoints;
        }

        return lengthEstimate;
    }
}

public class BezierSplineComponent : MonoBehaviour
{

    private BezierSpline spline;

    public Transform P0;
    public Transform P1;
    public Transform P2;
    public Transform P3;

    void Awake()
    {
        if (spline == null)
        {
            spline = new(P0, P1, P2, P3);
        }

        spline.P0 = P0;
        spline.P1 = P1;
        spline.P2 = P2;
        spline.P3 = P3;
    }

    public void SetPoints()
    {
        if (spline == null)
        {
            spline = new(P0, P1, P2, P3);
        }

        spline.P0 = P0;
        spline.P1 = P1;
        spline.P2 = P2;
        spline.P3 = P3;
    }

    public Vector3 Interpolate(float t)
    {
        if (spline == null) return P0.position;
        return spline.Interpolate(t);
    }
    public float EstimateLength()
    {
        if (spline == null) return 0;
        return spline.EstimateLength();
    }
}
