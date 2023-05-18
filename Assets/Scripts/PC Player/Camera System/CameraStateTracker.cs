using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateTracker : MonoBehaviour
{
    public List<Camera> Cameras;
    private static CameraStateTracker instance;

    public void Awake()
    {
        instance = this;
    }

    public static CameraStateTracker Instance()
    {
        return instance;
    }

    public void ChangeCameraState()
    {

    }
}
