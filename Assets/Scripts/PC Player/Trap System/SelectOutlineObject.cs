using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class SelectOutlineObject : MonoBehaviour
{
    [SerializeField] private GameObjectValueList cameraList;
    [SerializeField] private IntReference currentCameraIndex;
    [SerializeField] private RectTransform textureRectTransform;
    [SerializeField] private Camera pcPlayerCamera;
    [HideInInspector] public bool isMovable = false;

    private void Update()
    {
        // CheckClick();
    }

    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
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

            int artifactMask = LayerMask.GetMask("Selectable");

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, artifactMask))
            {
                print("colliders "+ hit.collider);
                Outline outline = hit.collider.GetComponent<Outline>();
                outline.enabled = !outline.enabled;
                isMovable = !isMovable;
                print("Selected!");
            }
        }
    }
}
