using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableToggle : MonoBehaviour
{
    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
            GetComponent<Collider>().enabled = !GetComponent<Collider>().enabled;
        }
    }

}
