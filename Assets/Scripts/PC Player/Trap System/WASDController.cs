using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WASDController : MonoBehaviour
{
    public float moveForceAmount = 0; //set move force amount in inspector
    private Rigidbody rB;

    void Start()
    {
        rB = GetComponent<Rigidbody>(); //get player rigidbody
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W)) //press W to move forward
        {
            rB.AddRelativeForce(0,0,moveForceAmount);
        }
        
        if(Input.GetKey(KeyCode.S)) //press S to move backward
        {
            rB.AddRelativeForce(0, 0, -moveForceAmount);
        }
        
        if(Input.GetKey(KeyCode.A)) //press A to turn counterclockwise
        {
            // rB.AddRelativeTorque(0,-rotateForceAmount,0); //I abandoned torque way cuz using DoTween to do that will be more stable
            rB.DORotate(Vector3.down, 0.05f).SetRelative();
        }
        
        if(Input.GetKey(KeyCode.D)) //press D to turn clockwise
        {
            // rB.AddRelativeTorque(0,rotateForceAmount,0);
            rB.DORotate(Vector3.up, 0.05f).SetRelative();
        }
    }
}
