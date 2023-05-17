using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using float_oat.Desktop90;
using UnityEngine.UI;


public class PCResourceManagement : MonoBehaviour
{
    public int maxWindow;
    public int currentWindow;
    public List<WindowController> activeWindows;
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
        currentWindow = activeWindows.Count;
        ratioInProgrss = (float)currentWindow / (float)maxWindow;
        progressFill.fillAmount = ratioInProgrss;
        progressText.text = "Active Window: " + currentWindow.ToString() + "     " + "Max Window: " + maxWindow.ToString();
    }

}
