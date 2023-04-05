using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class VRPlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private IntReference currentHealth, maxHealth;
    
    [HideInInspector] public bool isClicked = false;
    [SerializeField] float time, cdCounter;

    private void Start()
    {
        currentHealth = new IntReference(maxHealth.Value);
    }

    private void Update()
    {
        DisplayDamagedFeedback();
    }

    public void OnBeAttacked()
    {
        isClicked = true;
        if (currentHealth.Value > 0)
        {
            print(currentHealth.Value);
            currentHealth.Value--;
        }
    }

    private void DisplayDamagedFeedback()
    {
        if (isClicked)
        {
            cdCounter += Time.deltaTime;
            GetComponent<Renderer>().material.color = Color.red;
        }

        if(cdCounter > time)
        {
            isClicked = false;
            GetComponent<Renderer>().material.color = Color.white;
            cdCounter = 0;
        }
    }
}
