using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBreakDetect : MonoBehaviour
{
    [HideInInspector]
    public bool isConnected = true;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name +" hit!");
        if (collision.gameObject.CompareTag("Sword") && isConnected)
        {
            print("Sword Hit!");
            if (collision.relativeVelocity.magnitude >= 5)
            {
                Destroy(GetComponent<HingeJoint>());
                isConnected = false;
            }
            
        }
    }
}
