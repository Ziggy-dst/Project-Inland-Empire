using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class VRPlayerHealthSystem : MonoBehaviour
{
    private MeshRenderer artifactRenderer;
    [SerializeField] private IntReference currentHealth;
    [SerializeField] private float feedbackPeriod;

    private void Start()
    {
        artifactRenderer = GetComponent<MeshRenderer>();
    }

    public void OnBeAttacked()
    {
        if (currentHealth.Value > 0)
        {
            currentHealth.Value--;
            StartCoroutine(DisplayDamagedFeedback(feedbackPeriod));
        }
    }

    private IEnumerator DisplayDamagedFeedback(float feedbackPeriod)
    {
        print("turn to red");
        artifactRenderer.material.color = Color.red;
        yield return new WaitForSeconds(feedbackPeriod);
        artifactRenderer.material.color = Color.white;
    }
}
