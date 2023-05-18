using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ModifiedDisplayCamera : MonoBehaviour
{
    public Camera cameraToDisplay;

    private void Awake()
    {
        cameraToDisplay.enabled = false;
    }

    public void DisplayCamera()
    {
        print("display camera");
        cameraToDisplay.enabled = true;
        var rectTransform = GetComponent<RectTransform>();
        var renderTexture = new RenderTexture((int)rectTransform.rect.width, (int)rectTransform.rect.height, 16);
        cameraToDisplay.targetTexture = renderTexture;
        GetComponent<RawImage>().texture = renderTexture;
    }

    private void OnEnable()
    {
        DisplayCamera();
    }

    private void OnDisable()
    {
        cameraToDisplay.enabled = false;
    }
}
