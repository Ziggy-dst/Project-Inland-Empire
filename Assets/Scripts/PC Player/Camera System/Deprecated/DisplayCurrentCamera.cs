using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90.Examples;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DisplayCurrentCamera : MonoBehaviour
{
    [SerializeField] private GameObjectValueList cameraList;
    [SerializeField] private IntReference currentCameraIndex;
    [SerializeField] private IntValueList activeCameraIndexList;
    private Camera cameraToDisplay = default;

    public void CheckSwitchedCameraState()
    {
        // do not display when the current cam is disabled
        if (activeCameraIndexList.Contains(currentCameraIndex.Value)) DisplayCamera();
        else RemoveDisplayCamera();
    }

    public void DisplayCamera()
    {
        cameraToDisplay = cameraList[currentCameraIndex.Value].GetComponent<Camera>();
        var rectTransform = GetComponent<RectTransform>();
        var renderTexture = new RenderTexture((int)rectTransform.rect.width, (int)rectTransform.rect.height, 16);
        cameraToDisplay.targetTexture = renderTexture;
        GetComponent<RawImage>().texture = renderTexture;
    }

    public void RemoveDisplayCamera()
    {
        GetComponent<RawImage>().texture = null;
    }
}
