using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class VRUIContainer : MonoBehaviour
{
    [SerializeField] private StringVariable _currentUIState = null;
    [SerializeField] private List<StringReference> _visibleForStates = null;
    private void Start()
    {
        StateNameChanged(_currentUIState.Value);
    }

    public void OnEventRaised(string stateName)
    {
        StateNameChanged(stateName);
    }

    private void StateNameChanged(string stateName)
    {
        if ((LayerMask.GetMask("VRUI") & (9<<gameObject.layer)) != 0)
        {
            if (_visibleForStates.Exists((state) => state.Value == stateName))
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

}
