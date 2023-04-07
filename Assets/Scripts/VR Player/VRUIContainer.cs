using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class VRUIContainer : MonoBehaviour, IAtomListener<string>
{
    [SerializeField] private StringVariable _currentUIState = null;
    [SerializeField] private List<StringReference> _visibleForStates = null;

    private void Awake()
    {
        if (_currentUIState.Changed != null)
        {
            _currentUIState.Changed.RegisterListener(this);
        }
    }

    private void Start()
    {
        StateNameChanged(_currentUIState.Value);
    }

    private void OnDestroy()
    {
        if (_currentUIState.Changed != null)
        {
            _currentUIState.Changed.UnregisterListener(this);
        }
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
