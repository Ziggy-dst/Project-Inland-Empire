using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using UnityEngine.EventSystems;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField] private IntReference currentAmmoNum, maxAmmoNum;

    [SerializeField] private GameObjectValueList cameraList;
    [SerializeField] private IntReference currentCameraIndex;
    [SerializeField] private IntValueList activeCameraIndexList;

    private Text shootButtonText;
    private D90Button shootButton;

    [SerializeField] private BoolReference isInShootingMode = new BoolReference(false);
    [SerializeField] private RectTransform textureRectTransform;
    [SerializeField] private Camera pcPlayerCamera;

    private void Start()
    {
        shootButton = GetComponent<D90Button>();
        shootButtonText = GetComponentInChildren<Text>();
        // currentCamera = cameraList[currentCameraIndex.Value].GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        Shoot();
    }

    public void ClickShootButton()
    {
        if (!isInShootingMode) EnterShootingMode();
        else ExitShootingMode();
    }

    public void CheckButtonAvailability()
    {
        shootButton.interactable = activeCameraIndexList.Contains(currentCameraIndex.Value);
    }

    private void EnterShootingMode()
    {
        // change mode
        isInShootingMode.Value = true;
        // change button text
        shootButtonText.text = "Cancel";
    }

    public void ExitShootingMode()
    {
        isInShootingMode.Value = false;
        shootButtonText.text = "Shoot";
    }

    private void Shoot()
    {
        if (!isInShootingMode) return;

        if (Input.GetMouseButton(0))
        {
            if (currentAmmoNum.Value > 0)
            {
                Camera currentCamera = cameraList[currentCameraIndex.Value].GetComponent<Camera>();

                // get the point of the RawImage where clicked
                RectTransformUtility.ScreenPointToLocalPointInRectangle(textureRectTransform, Input.mousePosition, pcPlayerCamera, out Vector2 localClick);
                //My RawImage is 700x700 and the click coordinates are in range (-350,350) so I transform it to (0,700) to then normalize
                localClick.x = (textureRectTransform.rect.xMin * -1) - (localClick.x * -1);
                localClick.y = (textureRectTransform.rect.yMin * -1) - (localClick.y * -1);
                //normalize the click coordinates to get the viewport point to cast a Ray
                Vector2 viewportClick = new Vector2(localClick.x / textureRectTransform.rect.size.x, localClick.y / textureRectTransform.rect.size.y);
                //cast the ray from the camera which rends the texture
                Ray ray = currentCamera.ViewportPointToRay(new Vector3(viewportClick.x, viewportClick.y, 0));

                // check if the shooting position within the bound
                
                int artifactMask = LayerMask.GetMask("Grabbable");

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, artifactMask))
                {
                    print("colliders "+ hit.collider);
                    if(hit.collider.tag.Equals("RaycastObject"))
                    {
                        hit.collider.GetComponent<VRPlayerHealthSystem>().OnBeAttacked();
                    }
                }
                // reduce ammo
                currentAmmoNum.Value--;

                // play the shooting sound
                print("Shot!");
                // exit the shooting mode
                ExitShootingMode();
            }
            else
            {
                // play sound of warning
                print("no enough ammo");
            }
        }
    }
}
