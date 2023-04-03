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
    [SerializeField] private IntValueList disabledCameraIndexList;
    private Camera _cameraToDisplay = default;
    
    
    void Start()
    {
       DisplayCamera();
    }

    public void DisplayCamera()
    {
        // do not display when the current cam is disabled
        if (disabledCameraIndexList.Contains(currentCameraIndex.Value))
        {
            RemoveDisplayCamera();
            return;
        }
        
        _cameraToDisplay = cameraList[currentCameraIndex.Value].GetComponent<Camera>();
        var rectTransform = GetComponent<RectTransform>();
        var renderTexture = new RenderTexture((int)rectTransform.rect.width, (int)rectTransform.rect.height, 16);
        _cameraToDisplay.targetTexture = renderTexture;
        GetComponent<RawImage>().texture = renderTexture;
    }

    public void RemoveDisplayCamera()
    {
        GetComponent<RawImage>().texture = null;
    }
}
