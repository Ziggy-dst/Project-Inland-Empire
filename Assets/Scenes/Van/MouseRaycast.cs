using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycast : MonoBehaviour
{
    public Camera camera;
    public CameraManager cameraManager;

    private void Update()
    {
        // camera = cameraManager.camList[cameraManager.current];
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //Transform objectHit = hit.transform;
                //objectHit.position += Vector3.up;
                print("colliders "+ hit.collider);

                if(hit.collider.tag.Equals("RaycastObject"))
                {
                    hit.collider.GetComponent<RaycastObject>().isClicked = true;
                }
            }
        }

    }


}
