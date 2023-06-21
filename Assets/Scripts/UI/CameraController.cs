using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float Speed = 20;
    public float ZoomSpeed = 5;
    public float MinSize = 2;
    public float MaxSize = 10;
    private float minX = 0;
    private float minZ = 0;
    private float maxX;
    private float maxZ;
    private Vector2 motion;
    private float zoom;

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        MoveWithinBounds();

    }

    private void MoveCamera()
    {
        motion = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        zoom = Input.GetAxis("Mouse ScrollWheel");

        transform.Translate(motion * Speed * Time.deltaTime);
        Camera.main.orthographicSize -= zoom * ZoomSpeed;
    }

    private void MoveWithinBounds()
    {
        double cameraVerticalViewHalfWidth = Camera.main.orthographicSize;
        double cameraHorizontalViewHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        minX = (float)cameraHorizontalViewHalfWidth;
        minZ = (float)cameraVerticalViewHalfWidth;
        maxX = Terrain.activeTerrain.terrainData.size.x - (float)cameraHorizontalViewHalfWidth;
        maxZ = Terrain.activeTerrain.terrainData.size.z - (float)cameraVerticalViewHalfWidth;

        float size = Camera.main.orthographicSize;
        size = Mathf.Clamp(size, MinSize, MaxSize);
        Camera.main.orthographicSize = size;

        Vector3 v3 = transform.position;
        v3.x = Mathf.Clamp(v3.x, minX, maxX);
        v3.z = Mathf.Clamp(v3.z, minZ, maxZ);
        transform.position = v3;
    }
}
