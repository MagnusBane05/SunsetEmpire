using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    public bool selected;
    Vector3 offset;

    void Update()
    {
        if (selected)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition + offset;
        }
    }

    private void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selected = true;
        offset = transform.position - mousePosition;
    }

    private void OnMouseUp()
    {
        selected = false;
    }
}
