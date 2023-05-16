using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class TankManager : MonoBehaviour
{
    public Transform playerTransform;
    public Transform turretTransform;
    public Transform cannonTransform;
    private bool isHit = false;
    public Image armorBarImage;
    void Start()
    {
        
    }


    void Update()
    {
        if (transform.localScale.y > 0.1f)
        {        
            Vector3 turretTargetPos = new Vector3(playerTransform.position.x, turretTransform.position.y,
                playerTransform.position.z);
            turretTransform.LookAt(turretTargetPos);

            Vector3 cannonTargetPos = new Vector3(playerTransform.position.x, playerTransform.position.y + 1.5f,
                playerTransform.position.z);
            cannonTransform.LookAt(cannonTargetPos);
        }

        if (transform.localScale.y < 0)
        {
            transform.localScale = new Vector3(1, 0.01f, 1);
        }

        armorBarImage.fillAmount = transform.localScale.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Something Hit!");
        if (collision.gameObject.CompareTag("Hammer") && !isHit)
        {
            print("Hammer Hit!");
            isHit = true;
            transform.localScale += new Vector3(0, -0.1f, 0);
            Invoke("ResetHit",0.1f);
        }
    }

    private void ResetHit()
    {
        isHit = false;
    }
}
