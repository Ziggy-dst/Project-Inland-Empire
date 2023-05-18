using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using float_oat.Desktop90;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;


public class PCResourceManagement : MonoBehaviour
{
    public IntReference maxWindowNum;

    public IntReference activeWindowsNum;
    private static PCResourceManagement instance;
    public float ratioInProgrss;
    public Image progressFill;
    public Text progressText;

    private void Awake()
    {
        instance = this;
    }
    
    public static PCResourceManagement Instance()
    {
        return instance;
    }

    public void UpdateProgress()
    {
        ratioInProgrss = (float)activeWindowsNum.Value / (float)maxWindowNum.Value;
        progressFill.fillAmount = ratioInProgrss;
        progressText.text = "Active Window: " + activeWindowsNum.Value + "     " + "Max Window: " + maxWindowNum.Value;
    }

}
