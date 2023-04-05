using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCameraUIText : MonoBehaviour
{
    private Text camUIText;
    [SerializeField] private IntReference currentCameraIndex;

    private void Start()
    {
        camUIText = GetComponent<Text>();
        ChangeText();
    }

    public void ChangeText()
    {
        camUIText.text = $"Cam {currentCameraIndex.Value.ToString()}";
    }
}
