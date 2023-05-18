using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core.Player;
using UnityEngine;

public class CustomizedHVRPlayerController : HVRPlayerController
{
    protected override void Crouch()
    {
        if (Inputs.IsCrouchActivated)
        {
            GetComponent<WeaponSwitch>().SwitchWeapon();
        }
    }

    public void OnBulletHit()
    {
        MoveSpeed = 0.5f;
        Invoke("ResetMoveSpeed", 5f);
    }
    
    private void ResetMoveSpeed()
    {
        MoveSpeed = 1.5f;
    }

    public void OnEnterWater()
    {
        MoveSpeed = 0.5f;
        // Invoke("ResetMoveSpeed", 3f);
    }

    public void OnExitWater()
    {
        MoveSpeed = 1.5f;
    }
}
