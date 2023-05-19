using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;

public class DisplayVRPlayerHP : MonoBehaviour
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
        ammoText.text = $"VR Player HP: {currentAmmoNum.Value}/{maxAmmoNum.Value}";
    }
}
