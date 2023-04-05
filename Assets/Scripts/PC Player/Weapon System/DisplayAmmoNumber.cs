using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAmmoNumber : MonoBehaviour
{
    [SerializeField] private IntReference currentAmmoNum, maxAmmoNum;
    private Text ammoText;

    private void Start()
    {
        ammoText = GetComponent<Text>();
        DisplayNumber();
    }

    public void DisplayNumber()
    {
        ammoText.text = $"Ammo: {currentAmmoNum.Value}/{maxAmmoNum.Value}";
    }
}
