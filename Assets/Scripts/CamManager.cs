using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public List<GameObject> camList; //create a Camera List in inspector
    [HideInInspector] public int currentCam = 0; //set current Cam to Cam1
    // Start is called before the first frame update
    void Start()
    {
        currentCam = 0;
        camList[0].SetActive(true); //set only Cam1 activated
        for (int i = 1; i <= camList.Count; i++) //deactivate the rest
        {
            camList[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) //press right arrow to switch cam
        {
            if (currentCam+1 == camList.Count) //loop to first cam when pressing right arrow on last cam
            {
                camList[0].SetActive(true);
                camList[currentCam].SetActive(false);
                currentCam = 0;
            }
            else
            {
                camList[currentCam+1].SetActive(true);
                camList[currentCam].SetActive(false);
                currentCam = currentCam + 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) //press left arrow to switch cam
        {
            if (currentCam == 0) //loop to last cam when pressing left arrow on first cam
            {
                camList[camList.Count-1].SetActive(true);
                camList[currentCam].SetActive(false);
                currentCam = camList.Count - 1;
            }
            else
            {
                camList[currentCam-1].SetActive(true);
                camList[currentCam].SetActive(false);
                currentCam = currentCam - 1;
            }
        }
    }
}
