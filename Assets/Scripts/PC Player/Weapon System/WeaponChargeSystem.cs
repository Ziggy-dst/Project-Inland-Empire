using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChargeSystem : MonoBehaviour
{
    private float chargeProgressPercent = 0;
    private bool isCountingDown = false;
    private D90Button chargeButton;

    [SerializeField] private Image progressBar;
    [SerializeField] private float percentReduceSpeed = 20;

    [SerializeField] private IntReference currentAmmoNum;
    [SerializeField] private IntReference maxAmmoNum;

    private void Start()
    {
        chargeButton = GetComponent<D90Button>();
    }

    // Update is called once per frame
    void Update()
    {
        ReduceProgress();
        DisplayPercent();
    }

    private void ReduceProgress()
    {
        if (isCountingDown)
        {
            if (chargeProgressPercent > 0)
            {
                chargeProgressPercent -= Time.deltaTime * percentReduceSpeed;
            }
            else
            {
                chargeProgressPercent = 0;
                isCountingDown = false;
            }
        }
    }

    public void ChargeForAmmo(float singleClickPercent)
    {
        // if (!canCharge) return;
        if (currentAmmoNum.Value >= maxAmmoNum.Value) return;
        if (chargeProgressPercent < 100)
        {
            print("is charging");
            chargeProgressPercent += singleClickPercent;
            isCountingDown = true;
        }
        else
        {
            print("charging finished");
            chargeProgressPercent = 100;
            isCountingDown = false;
            ChangeButtonState(false);
            AddAmmo();
        }
    }

    private void AddAmmo()
    {
        print("ammo added");
        currentAmmoNum.Value++;
        if (currentAmmoNum.Value == maxAmmoNum.Value) ChangeButtonState(false);
        else StartCoroutine(ResetProgress(2f));
    }

    public void ResetAfterFull()
    {
        if (currentAmmoNum.Value == maxAmmoNum.Value - 1) StartCoroutine(ResetProgress(0.5f));
    }

    private IEnumerator ResetProgress(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        // yield return new WaitUntil(() =>
        // {
        //     while (chargeProgressPercent > 0)
        //     {
        //         chargeProgressPercent -= Time.deltaTime * percentReduceSpeed;
        //         // if (chargeProgressPercent <= 0) return false;
        //     }
        //     return true;
        // });
        chargeProgressPercent = 0;
        ChangeButtonState(true);
    }

    private void DisplayPercent()
    {
        progressBar.fillAmount = chargeProgressPercent / 100;
    }

    private void ChangeButtonState(bool isInteractable)
    {
        chargeButton.interactable = isInteractable;
    }
}
