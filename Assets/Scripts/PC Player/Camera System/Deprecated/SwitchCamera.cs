using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private GameObjectValueList cameraList;
    [SerializeField] private IntReference currentCameraIndex = new IntReference(0);

    private void Awake()
    {
        Assert.IsNotNull(cameraList);
        Assert.IsNotNull(currentCameraIndex);
    }
    
    public void SwitchToNextCamera()
    {
        if (currentCameraIndex.Value + 1 == cameraList.Count) //loop to first cam when pressing right arrow on last cam
        {
            currentCameraIndex.Value = 0;
        }
        else
        {
            currentCameraIndex.Value += 1;
        }
    }

    public void SwitchToLastCamera()
    {
        if (currentCameraIndex.Value == 0) //loop to last cam when pressing left arrow on first cam
        {
            currentCameraIndex.Value = cameraList.Count - 1;
        }
        else
        {
            currentCameraIndex.Value -= 1;
        }
    }
}
