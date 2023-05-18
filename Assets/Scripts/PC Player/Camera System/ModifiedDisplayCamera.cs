using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ModifiedDisplayCamera : MonoBehaviour
{
    public Camera cameraToDisplay;

    public void DisplayCamera()
    {
        var rectTransform = GetComponent<RectTransform>();
        var renderTexture = new RenderTexture((int)rectTransform.rect.width, (int)rectTransform.rect.height, 16);
        cameraToDisplay.targetTexture = renderTexture;
        GetComponent<RawImage>().texture = renderTexture;
    }

    private void Awake()
    {
        DisplayCamera();
    }
}
