using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycast : MonoBehaviour
{
    public Camera camera;
    public CamManager camM;

    private void Update()
    {
        camera = camM.camList[camM.currentCam];
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

                if(hit.collider.tag == "RaycastObject")
                {
                    hit.collider.GetComponent<RaycastObject>().isClicked = true;
                }
            }
        }

    }


}
