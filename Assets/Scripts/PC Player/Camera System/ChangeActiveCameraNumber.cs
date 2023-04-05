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
    [SerializeField] private IntReference maxActiveCameraNum;

    private void Start()
    {
        numText = GetComponent<Text>();
        ChangeCameraNumber();
    }

    public void ChangeCameraNumber()
    {
        numText.text = $"Active Camera: {activeCameraIndexList.Count.ToString()}/{maxActiveCameraNum.Value}";
    }
}
