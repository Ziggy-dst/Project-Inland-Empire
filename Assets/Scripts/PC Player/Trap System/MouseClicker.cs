using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClicker : MonoBehaviour
{

    private void OnClick()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Camera currentCamera = cameraList[currentCameraIndex.Value].GetComponent<Camera>();
        //
        //     // get the point of the RawImage where clicked
        //     RectTransformUtility.ScreenPointToLocalPointInRectangle(textureRectTransform, Input.mousePosition, pcPlayerCamera, out Vector2 localClick);
        //     //My RawImage is 700x700 and the click coordinates are in range (-350,350) so I transform it to (0,700) to then normalize
        //     localClick.x = (textureRectTransform.rect.xMin * -1) - (localClick.x * -1);
        //     localClick.y = (textureRectTransform.rect.yMin * -1) - (localClick.y * -1);
        //     //normalize the click coordinates to get the viewport point to cast a Ray
        //     Vector2 viewportClick = new Vector2(localClick.x / textureRectTransform.rect.size.x, localClick.y / textureRectTransform.rect.size.y);
        //     //cast the ray from the camera which rends the texture
        //     Ray ray = currentCamera.ViewportPointToRay(new Vector3(viewportClick.x, viewportClick.y, 0));
        //
        //     // check if the shooting position within the bound
        //
        //     int artifactMask = LayerMask.GetMask("Grabbable");
        //
        //     if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, artifactMask))
        //     {
        //         print("colliders "+ hit.collider);
        //         if(hit.collider.tag.Equals("RaycastObject"))
        //         {
        //             hit.collider.GetComponent<VRPlayerHealthSystem>().OnBeAttacked();
        //         }
        //     }
        //     // reduce ammo
        //     currentAmmoNum.Value--;
        //
        //     // play the shooting sound
        //     print("Shot!");
        //     // exit the shooting mode
        //     ExitShootingMode();
        // }
    }
}
