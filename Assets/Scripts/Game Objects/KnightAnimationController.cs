using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimationController : MonoBehaviour, IShootable
{
    private Animator knightAnimator;
    
    // public int knightRotationOffset = 0;

    public Transform playerTransform;

    private HPBarManager hpBarManager;

    private float hP;

    void Start()
    {
        knightAnimator = GetComponent<Animator>();
        hpBarManager = GetComponent<HPBarManager>();
    }
    
    void Update()
    {
        hP = hpBarManager.hP;

        if (knightAnimator.GetCurrentAnimatorStateInfo(0).IsName("Blocking"))
        {
            knightAnimator.ResetTrigger("isHit");
        }
        

        if (hP >= hpBarManager.totalHP)
        {
            Vector3 targetPos = new Vector3(playerTransform.position.x, this.transform.position.y,
                playerTransform.position.z);
            transform.LookAt(targetPos);
        }

        if (hP < hpBarManager.totalHP)
        {
            if (knightAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                transform.rotation = Quaternion.Euler(0,30,0);
                knightAnimator.SetBool("isDamaged", true);
                // knightAnimator.SetTrigger("isAtLeft");
            }

            // if (!knightAnimator.GetBool("isAtLeft"))
            // {
            //     knightAnimator.SetTrigger("isAtRight");
            //     transform.rotation = Quaternion.Euler(0,180,0);
            // }
            //
            // if (!knightAnimator.GetBool("isAtRight"))
            // {
            //     knightAnimator.SetTrigger("isAtLeft");
            //     transform.rotation = Quaternion.Euler(0,0,0);
            // }
        }

        if (hP <= 0)
        {
            knightAnimator.SetBool("isDead", true);
        }

        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     OnBeAttacked();
        // }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // print("hit something!");
        if (collision.gameObject.CompareTag("Sword") && knightAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            knightAnimator.SetTrigger("isHit");
            // transform.rotation = Quaternion.Euler(0,
            //     Quaternion.LookRotation(collision.GetContact(0).normal).eulerAngles.y + knightRotationOffset, 0);
            // print("hit knight!");
        }
        // print(collision.gameObject.name);
    }

    public void OnBeAttacked()
    {
        knightAnimator.SetFloat("SpeedMultiplier", 2f);
        Invoke("ResetSpeed", 5f);
    }

    private void ResetSpeed()
    {
        knightAnimator.SetFloat("SpeedMultiplier", 1f);
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Bullet") && knightAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
    //     {
    //         hP -= 5;
    //         print(hP);
    //     }
    // }
}
