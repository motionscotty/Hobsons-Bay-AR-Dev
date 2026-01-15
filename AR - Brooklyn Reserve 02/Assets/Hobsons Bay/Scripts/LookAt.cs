using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
   
    public Transform targetCamera;  // Assign AR Camera here

    void Update()
    {
        if (targetCamera != null)
        {
            transform.LookAt(targetCamera);  // Points forward at camera
        }
    }
}


