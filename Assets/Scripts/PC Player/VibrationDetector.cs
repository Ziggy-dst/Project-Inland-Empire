using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using float_oat.Desktop90;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Grabbers;

public class VibrationDetector : MonoBehaviour
{
    public Transform VRTransform;

    private static VibrationDetector instance;

    public HVRHandGrabber lefthand;
    public HVRHandGrabber righthand;

    public void Awake()
    {
        instance = this;
    }

    public static VibrationDetector Instance()
    {
        return instance;
    }


    //Return the distance as a magnitude of the difference between two Vector3s.  
    public void DistanceDetectForVibration(Camera newestCamera)
    {
        if (VRTransform == null) return;
        Vector3 vrVector = VRTransform.position;
        Vector3 cameraVector = newestCamera.transform.position;

        Vector3 distance = vrVector - cameraVector;
        
        print(distance.magnitude);

        VibrateVRHands(10/distance.magnitude, 6/distance.magnitude, 10000); // TBD
    
    }
    public void VibrateVRHands(float amplitude, float duration, float frequency)
    {
        righthand.Controller.Vibrate(amplitude, duration, frequency);
        lefthand.Controller.Vibrate(amplitude, duration, frequency);
    }

}
