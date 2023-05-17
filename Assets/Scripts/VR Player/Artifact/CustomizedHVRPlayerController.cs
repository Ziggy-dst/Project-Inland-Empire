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
}
