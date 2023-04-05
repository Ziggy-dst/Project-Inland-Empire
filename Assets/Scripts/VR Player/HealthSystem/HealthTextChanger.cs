using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using TMPro;

public class HealthTextChanger : MonoBehaviour
{
    private TextMeshPro healthText;
    [SerializeField] private IntReference currentHealth;

    private void Start()
    {
        healthText = GetComponent<TextMeshPro>();
        DisplayNumber();
    }

    public void DisplayNumber()
    {
        healthText.text = currentHealth.Value.ToString();
    }
}
