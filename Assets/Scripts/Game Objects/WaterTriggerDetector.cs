using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollideDetector : MonoBehaviour
{
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerTarget"))
        {
            other.GetComponent<CustomizedHVRPlayerController>().OnEnterWater();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerTarget"))
        {
            other.GetComponent<CustomizedHVRPlayerController>().OnExitWater();
        } 
    }
}
