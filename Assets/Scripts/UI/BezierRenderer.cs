using UnityEngine;

[ExecuteAlways]
public class BezierRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public BezierSplineComponent BezierSpline;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        BezierSpline = GetComponent<BezierSplineComponent>();
    }

    void Update()
    {
        if (BezierSpline.P0 == null || BezierSpline.P3 == null) return;

        DrawCubicBezierCurve();
    }

    void DrawCubicBezierCurve()
    {

        lineRenderer.positionCount = 200;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            B = BezierSpline.Interpolate(t);
            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
        }
    }
}