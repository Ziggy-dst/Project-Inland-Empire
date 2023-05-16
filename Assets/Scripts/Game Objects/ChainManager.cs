using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainManager : MonoBehaviour
{
    [HideInInspector]
    public bool isChained = true;

    private ChainBreakDetect[] ChainBreakDetects;
    
    void Start()
    {
        ChainBreakDetects = GetComponentsInChildren<ChainBreakDetect>();
    }


    void Update()
    {
        if (isChained)
        {
            foreach (var chainBreakDetect in ChainBreakDetects)
            {
                if (chainBreakDetect.isConnected == false)
                {
                    isChained = false;
                    foreach (var chainBreakDetector in ChainBreakDetects)
                    {
                        Destroy(chainBreakDetector);
                    }
                }
            }
        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     print(collision.gameObject.name +" hit!");
    //     if (isConnected && collision.gameObject.CompareTag("Sword"))
    //     {
    //         print("Sword Hit!");
    //         if (collision.relativeVelocity.magnitude >= 3)
    //         {
    //             isConnected = false;
    //             Collider brokenChain = collision.GetContact(0).thisCollider;
    //             brokenChain.gameObject.GetComponent<HingeJoint>().connectedBody = null;
    //         }
    //         
    //     }
    // }
}
