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
    [SerializeField] private IntValueList disabledCameraIndexList; //TO DO

    private void Awake()
    {
        Assert.IsNotNull(tickImage);
        Assert.IsNotNull(cameraImage);
        Assert.IsNotNull(cameraList);
        Assert.IsNotNull(currentCameraIndex);
        Assert.IsNotNull(disabledCameraIndexList);
    }

    private void Start()
    {
        // disable all cam at the beginning
        // for (int i = 0; i < 9; i++)
        // {
        //     disabledCameraIndexList.Add(i);
        // }
    }

    public void ChangeCameraState()
    {
        if (disabledCameraIndexList.Contains(currentCameraIndex)) EnableCamera();
        else DisableCamera();
        // ChangeButtonState();
    }

    public void EnableCamera()
    {
        if (disabledCameraIndexList.Count >= 3) disabledCameraIndexList.Remove(currentCameraIndex);
    }

    public void DisableCamera()
    {
        disabledCameraIndexList.Add(currentCameraIndex.Value);
    }

    public void CheckCurrentCameraState()
    {
        if (disabledCameraIndexList.Contains(currentCameraIndex.Value)) tickImage.enabled = false;
        else tickImage.enabled = true;
    }
    
    // public void ChangeButtonState()
    // {
    //     tickImage.enabled = !tickImage.enabled;
    // }
}
