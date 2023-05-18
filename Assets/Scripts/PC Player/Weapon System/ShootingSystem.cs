using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.EventSystems;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField] private IntReference currentAmmoNum;

    [SerializeField] private BoolReference isInShootingMode = new BoolReference(false);
    private RectTransform currentCameraWindow;

    [SerializeField] private MMF_Player shootFeedback;

    private void Update()
    {
        CursorBoundCheck();
        CheckIfInShootingMode();
        Shoot();
    }

    private void CursorBoundCheck()
    {
        // if cursor is outside the bound of the current camera window
        if (!RectTransformUtility.RectangleContainsScreenPoint(currentCameraWindow, Input.mousePosition))
        {
            if (isInShootingMode)
            {
                // print("cursor outside");
                // exit the shooting mode
                ExitShootingMode();
            }
        }
    }

    private void CheckIfInShootingMode()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            // check if the cursor is over a window
            // If there are any results, print the name of the first (topmost) GameObject
            if (results.Count > 0)
            {
                // print("rect exists");
                RectTransform rectTransform = results[0].gameObject.GetComponent<RectTransform>();
                // Debug.Log("Right-clicked over: " + rectTransform.name);

                if (rectTransform == null) return;
                // check if the window is a camera window
                if (rectTransform.tag.Equals("CameraWindow"))
                {
                    // print("camera rect exists");
                    // if yes, set it as the current camera window
                    currentCameraWindow = rectTransform;
                    if (isInShootingMode) ExitShootingMode();
                    else EnterShootingMode();
                    // Debug.Log("Right-clicked over: " + rectTransform.name);
                }
            }
        }
    }

    private void EnterShootingMode()
    {
        isInShootingMode.Value = true;
    }

    public void ExitShootingMode()
    {
        isInShootingMode.Value = false;
    }

    private void Shoot()
    {
        if (!isInShootingMode) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmoNum.Value > 0)
            {
                shootFeedback.PlayFeedbacks();

                Camera currentCamera = currentCameraWindow.GetComponent<ModifiedDisplayCamera>().cameraToDisplay;
                // print(currentCameraWindow.rect);

                // get the point of the RawImage where clicked
                RectTransformUtility.ScreenPointToLocalPointInRectangle(currentCameraWindow, Input.mousePosition, null, out Vector2 localClick);
                //My RawImage is 700x700 and the click coordinates are in range (-350,350) so I transform it to (0,700) then normalize
                localClick.x = (currentCameraWindow.rect.xMin * -1) - (localClick.x * -1);
                localClick.y = (currentCameraWindow.rect.yMin * -1) - (localClick.y * -1);
                //normalize the click coordinates to get the viewport point to cast a Ray
                Vector2 viewportClick = new Vector2(localClick.x / currentCameraWindow.rect.size.x, localClick.y / currentCameraWindow.rect.size.y);
                //cast the ray from the camera which rends the texture
                Ray ray = currentCamera.ViewportPointToRay(new Vector3(viewportClick.x, viewportClick.y, 0));

                // print("local click " + localClick);
                // print("view port " + viewportClick);

                // TODO: identify other shootable objects: task, lights
                // use IShootable interface

                int artifactMask = LayerMask.GetMask("Grabbable", "Shootable", "RaycastOnly");
                

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, artifactMask))
                {
                    print("colliders "+ hit.collider);
                    if (hit.collider.CompareTag("Sword") || hit.collider.CompareTag("Hammer") ||
                        hit.collider.CompareTag("Gun") || hit.collider.CompareTag("RootCube") ||
                        hit.collider.CompareTag("Knight") || hit.collider.CompareTag("Police")) 
                    {
                        if (hit.collider.GetComponent<IShootable>() == null)
                        {
                            hit.collider.GetComponentInParent<IShootable>().OnBeAttacked();
                        }
                        else
                        {
                            hit.collider.GetComponent<IShootable>().OnBeAttacked();
                        }
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
