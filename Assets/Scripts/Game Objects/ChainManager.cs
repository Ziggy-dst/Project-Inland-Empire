using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityAtoms.Editor;
using UnityEngine;

public class ChainManager : MonoBehaviour
{
    [HideInInspector]
    public bool isChained = true;

    private ChainBreakDetect[] ChainBreakDetects;

    public IntVariable remainingChains;
    
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
                    remainingChains.Value--;
                    foreach (var chainBreakDetector in ChainBreakDetects)
                    {
                        Destroy(chainBreakDetector);
                        break;
                    }
                    break;
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
