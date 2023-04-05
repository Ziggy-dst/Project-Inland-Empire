using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timer; //get 2 UI text
    public TextMeshProUGUI camText;
    public CameraManager cameraManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer.text = System.DateTime.Now.ToString(); //add system time to screen
        // camText.text = "CCTVM-CAMERA " + (cameraManager. + 1).ToString(); //add current cam info to screen
    }
}