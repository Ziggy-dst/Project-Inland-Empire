using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class WASDController : MonoBehaviour
{
    public float moveForceAmount = 0; //set move force amount in inspector
    private Rigidbody rb;
    [SerializeField] private SelectOutlineObject selectOutlineObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); //get player rigidbody
    }

    void FixedUpdate()
    {
       Move();
    }

    private void Move()
    {
        if (!selectOutlineObject.isMovable) return;
        if (Input.GetKey(KeyCode.W)) //press W to move forward
        {
            rb.AddRelativeForce(0,0,moveForceAmount);
        }

        if(Input.GetKey(KeyCode.S)) //press S to move backward
        {
            rb.AddRelativeForce(0, 0, -moveForceAmount);
        }

        if(Input.GetKey(KeyCode.A)) //press A to turn counterclockwise
        {
            // rB.AddRelativeTorque(0,-rotateForceAmount,0); //I abandoned torque way cuz using DoTween to do that will be more stable
            rb.DORotate(Vector3.down, 0.05f).SetRelative();
        }

        if(Input.GetKey(KeyCode.D)) //press D to turn clockwise
        {
            // rB.AddRelativeTorque(0,rotateForceAmount,0);
            rb.DORotate(Vector3.up, 0.05f).SetRelative();
        }
    }
}
