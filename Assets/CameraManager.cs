using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<Camera> camList = new List<Camera>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var cam in camList)
        {
            cam.enabled = true;
        }
    }
}
