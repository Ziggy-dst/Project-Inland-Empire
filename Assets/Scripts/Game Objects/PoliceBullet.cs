using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Shared;
using UnityEngine;

public class PoliceBullet : MonoBehaviour
{
    private CustomizedHVRPlayerController playerController;
    
    void Start()
    {
        Invoke("SelfDestroy", 8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTarget"))
        {
            other.GetComponent<CustomizedHVRPlayerController>().OnBulletHit();
            Destroy(gameObject);
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
