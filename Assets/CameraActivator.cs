using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActivator : MonoBehaviour
{
    void Start()
    {
        GetComponent<Camera>().enabled = true;
    }
}
