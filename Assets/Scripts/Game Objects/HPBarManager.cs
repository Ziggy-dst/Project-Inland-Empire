using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Components;
using UnityEngine;
using UnityEngine.UI;

public class HPBarManager : MonoBehaviour
{
    public Image hPBarImage1;
    // public Image hPBarImage2;
    // public Image hPBarImage3;
    
    [HideInInspector]
    public float hP;

    [HideInInspector]
    public float totalHP;
    
    // private KnightAnimationController kACinstance;

    private HVRDamageHandler[] damageHandlers;

    // Start is called before the first frame update
    void Start()
    {
        // kACinstance = GetComponent<KnightAnimationController>();
        damageHandlers = GetComponentsInChildren<HVRDamageHandler>();
        foreach (var damageHandler in damageHandlers)
        {
            totalHP += damageHandler.Life;
        }

        hP = totalHP;
    }

    // Update is called once per frame
    void Update()
    {
        hP = 0;
        foreach (var damageHandler in damageHandlers)
        {
            hP += damageHandler.Life;
        }
        // print(hP);

        if (hP >= 0)
        {
            hPBarImage1.fillAmount = hP / totalHP;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var damageHandler in damageHandlers)
            {
                damageHandler.Life--;
            }
        }
        // if (hP >= 100 / 3 * 2)
        // {
        //     hPBarImage1.fillAmount = (hP - 100 * 2 / 3) / (100 / 3);
        // }
        //
        // if (hP < 100 / 3 * 2 && hP >= 100 / 3)
        // {
        //     hPBarImage2.fillAmount = (hP - 100 / 3) / (100 / 3);
        // }
        //
        // if (hP < 100 / 3 && hP >= 100)
        // {
        //     hPBarImage3.fillAmount = hP / (100 / 3);
        // }
    }
}
