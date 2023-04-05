using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;

public class HealthTextChanger : MonoBehaviour
{
    [SerializeField] private IntReference currentHealth;
    private Text _healthText;

    private void Start()
    {
        _healthText = GetComponent<Text>();
        DisplayNumber();
    }

    public void DisplayNumber()
    {
        print("displayed");
        _healthText.text = currentHealth.Value.ToString();
    }
}
