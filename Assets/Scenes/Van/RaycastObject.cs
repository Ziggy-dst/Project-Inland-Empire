using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObject : MonoBehaviour
{
    public bool isClicked = false;

    public float time;
    public float cdCounter;

    private void Update()
    {
        if (isClicked)
        {
            cdCounter += Time.deltaTime;

            GetComponent<Renderer>().material.color = Color.red;

            
        }

        if(cdCounter > time)
        {
            isClicked = false;
            GetComponent<Renderer>().material.color = Color.white;
            cdCounter = 0;
        }
    }
}
