using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
public class Debugger : MonoBehaviour
{
    public float initialTime = 500;

    public float current;
    public IntReference currentTime;

    public float ratio;

    public bool isDebugging;

    public float minute;
    public float sec;

    public Text timeLeft;
    public Image debugProgressFill;


    private static Debugger instance;


    private void Awake()
    {
        instance = this;
        current = currentTime.Value;
    }

    public static Debugger Instance()
    {
        return instance;
    }

    public void getRatio()
    {
        ratio = currentTime / initialTime;
    }

    public int secToMinute()
    {
        minute = current / 60;
        sec = current % 60;
        sec = (int)sec;
        currentTime.Value = (int)current;

        return (int)minute;
    }

    private void FixedUpdate()
    {
        if (isDebugging)
        {
            current -= Time.deltaTime;
            getRatio();
            debugProgressFill.fillAmount = 1 - ratio;
            timeLeft.text = "Time Left: " + secToMinute().ToString() + " M " + sec.ToString() + " S";
        }
    }
}
