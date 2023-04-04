using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class ChangeActiveCameraNumber : MonoBehaviour
{
    private Text numText;
    [SerializeField] private IntValueList activeCameraIndexList;

    private void Start()
    {
        numText = GetComponent<Text>();
    }

    public void ChangeCameraNumber()
    {
        numText.text = activeCameraIndexList.Count.ToString();
    }

    // public void AddActiveCameraNumber()
    // {
    //     int currentTotalNum = int.Parse(numText.text);
    //     currentTotalNum++;
    //     numText.text = currentTotalNum.ToString();
    // }
    //
    // public void ReduceActiveCameraNumber()
    // {
    //     int currentTotalNum = int.Parse(numText.text);
    //     currentTotalNum--;
    //     numText.text = currentTotalNum.ToString();
    // }
}
