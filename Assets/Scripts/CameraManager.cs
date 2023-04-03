using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObjectValueList cameraList; //create a Camera List in inspector

    private void Awake()
    {
        Assert.IsNotNull(cameraList);
    }

    // void Start()
    // {
    //     foreach (var cam in cameraList)
    //     {
    //         cam.SetActive(true);
    //     }
    // }
}
