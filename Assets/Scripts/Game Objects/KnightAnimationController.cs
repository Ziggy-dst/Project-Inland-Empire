using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimationController : MonoBehaviour
{
    private Animator knightAnimator;
    
    public int knightRotationOffset = 0;

    void Start()
    {
        knightAnimator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (knightAnimator.GetCurrentAnimatorStateInfo(0).IsName("Blocking"))
        {
            knightAnimator.ResetTrigger("isHit");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // print("hit something!");
        if (collision.gameObject.CompareTag("Sword") && knightAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            knightAnimator.SetTrigger("isHit");
            transform.rotation = Quaternion.Euler(0,
                Quaternion.LookRotation(collision.GetContact(0).normal).eulerAngles.y + knightRotationOffset, 0);
            // print("hit knight!");
        }
        
        print(collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet") && knightAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            knightAnimator.SetBool("isDead", true);
        }
    }
}
