using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerHealthSystem : MonoBehaviour
{
    [HideInInspector] public bool isClicked = false;

    [SerializeField] float time, cdCounter;

    private void Update()
    {
        DisplayDamagedFeedback();
    }

    private void DisplayDamagedFeedback()
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
