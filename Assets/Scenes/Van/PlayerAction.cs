using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public GameObject handObject;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowTheObject();
        }

        Ray throwRay = new Ray(transform.position, transform.eulerAngles);
        Debug.DrawRay(transform.position, transform.forward,Color.red);

    }



    public void ThrowTheObject()
    {

        handObject.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 100);
        handObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
