using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ChangeCurrentCameraState : MonoBehaviour
{
    [SerializeField] private Image tickImage;
    [SerializeField] private RawImage cameraImage;
    [SerializeField] public GameObjectValueList cameraList;
    [SerializeField] private IntReference currentCameraIndex = new IntReference(0);
    [SerializeField] private IntValueList activeCameraIndexList;

    private void Awake()
    {
        Assert.IsNotNull(tickImage);
        Assert.IsNotNull(cameraImage);
        Assert.IsNotNull(cameraList);
        Assert.IsNotNull(currentCameraIndex);
        Assert.IsNotNull(activeCameraIndexList);

        activeCameraIndexList.Clear();
    }

    public void ChangeCameraState()
    {
        if (!activeCameraIndexList.Contains(currentCameraIndex)) EnableCamera();
        else DisableCamera();
    }

    public void EnableCamera()
    {
        if (activeCameraIndexList.Count < cameraList.Count - 3)
        {
            activeCameraIndexList.Add(currentCameraIndex);
            tickImage.enabled = true;
        }
    }

    public void DisableCamera()
    {
        activeCameraIndexList.Remove(currentCameraIndex.Value);
        tickImage.enabled = false;
    }

    public void ChangeButtonState()
    {
        if (activeCameraIndexList.Contains(currentCameraIndex.Value)) tickImage.enabled = true;
        else tickImage.enabled = false;
    }
}
