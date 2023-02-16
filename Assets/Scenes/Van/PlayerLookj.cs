using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookj : MonoBehaviour
{
    public Transform playerCamera;
    public Vector2 sensitivities;

    private Vector2 XYRoatation;

    private void Start()
    {
        
    }

    private void Update()
    {
        Vector2 mouseInput = new Vector2
        {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        XYRoatation.x -= mouseInput.y * sensitivities.y;
        XYRoatation.y += mouseInput.x * sensitivities.x;

        XYRoatation.x = Mathf.Clamp(XYRoatation.x, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, XYRoatation.y, 0);
        playerCamera.localEulerAngles = new Vector3(XYRoatation.x, 0f, 0f);
    }
}
