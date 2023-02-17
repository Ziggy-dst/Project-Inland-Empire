using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActivator : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Camera>().enabled = true;
    }
}
