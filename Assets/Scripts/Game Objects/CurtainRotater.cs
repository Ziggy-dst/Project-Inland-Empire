using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityAtoms.BaseAtoms;

public class CurtainRotater : MonoBehaviour
{
    public GameObject curtain;
    public IntVariable remainingChains;
    private bool isRotated = false;
    
    void Start()
    {
        curtain = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingChains.Value == 0 && !isRotated)
        {
            RotateCurtain();
            isRotated = true;
        }
    }

    public void RotateCurtain()
    {
        curtain.transform.DORotate(new Vector3(90, 0, 0), 2f);
    }
}
