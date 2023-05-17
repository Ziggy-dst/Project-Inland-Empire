using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.HandPoser;
using HurricaneVR.Framework.Shared;
using System.Linq;

// attach to VR player
// each button on the right hand relates to a weapon, call the SwitchWeapon method to switch weapon
public class WeaponSwitch : MonoBehaviour
{
    // including the cube model
    [SerializeField] private List<GameObject> WeaponList;
    private GameObject currentWeapon;

    public HVRHandGrabber Grabber { get; set; }
    private HVRGrabbable Grabbable;
    public HVRGrabTrigger GrabTrigger;
    public HVRPosableGrabPoint GrabPoint;

    private void Start()
    {
        currentWeapon = WeaponList[0];
        Grabber = FindObjectsOfType<HVRHandGrabber>().FirstOrDefault(e => e.gameObject.activeInHierarchy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) SwitchWeapon(0);
    }

    public void SwitchWeapon(int index)
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
            Destroy(currentWeapon);
            currentWeapon = Instantiate(WeaponList[index], transform.position, Quaternion.identity);
            Grabbable = currentWeapon.GetComponent<HVRGrabbable>();
            Grab();
            Debug.Log("weapon " + index + " activated");
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     Destroy(currentWeapon);
        //     currentWeapon = Instantiate(WeaponList[1], transform.position, Quaternion.identity);
        //     Grabbable = currentWeapon.GetComponent<HVRGrabbable>();
        //     Grab();
        //     Debug.Log("1 clicked");
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     Destroy(currentWeapon);
        //     currentWeapon = Instantiate(WeaponList[2], transform.position, Quaternion.identity);
        //     Grabbable = currentWeapon.GetComponent<HVRGrabbable>();
        //     Grab();
        //     Debug.Log("2 clicked");
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     Destroy(currentWeapon);
        //     currentWeapon = Instantiate(WeaponList[3], transform.position, Quaternion.identity);
        //     Grabbable = currentWeapon.GetComponent<HVRGrabbable>();
        //     Grab();
        //     Debug.Log("3 clicked");
        // }
    }

    private void Grab()
    {
        if (Grabbable && Grabber)
        {
            if (GrabTrigger == HVRGrabTrigger.ManualRelease && Grabber.GrabbedTarget == Grabbable)
            {
                Grabber.ForceRelease();
                return;
            }

            //grabber needs to have it's release sequence completed if it's holding something
            if(Grabber.IsGrabbing)
                Grabber.ForceRelease();
            Grabber.Grab(Grabbable, GrabTrigger, GrabPoint);
        }
    }

    private void StartSwitchTransitionEffect()
    {
        // TODO
    }
}
