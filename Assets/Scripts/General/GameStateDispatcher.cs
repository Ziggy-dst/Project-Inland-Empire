using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityAtoms.FSM;

public class GameStateDispatcher : MonoBehaviour
{
    [TableList]
    [SerializeField] private List<FSMListReferenceEventPair> fsmListReferenceEventPair = new List<FSMListReferenceEventPair>();
    [SerializeField] private IntReference currentValue, limitValue;
    [SerializeField] private Comparator comparator;
    
    private enum Comparator
    {
        GreaterThan,
        SmallerThan,
        Equal
    }

    private bool Compare(int _currentValue, int _limitValue, Comparator _comparator)
    {
        switch (_comparator)
        {
            case Comparator.GreaterThan:
                if (_currentValue > _limitValue) return true;
                return false;
            case Comparator.SmallerThan:
                if (_currentValue < _limitValue) return true;
                return false;
            case Comparator.Equal:
                if (_currentValue == _limitValue) return true;
                return false;
            default:
                return false;
        }
    }

    public void Dispatch()
    {
        if (Compare(currentValue.Value, limitValue.Value, comparator))
        {
            foreach (var pair in fsmListReferenceEventPair)
            {
                pair.fsmListReference.Machine.Dispatch(pair.responseEvent);
            }
        }
    }
}

[Serializable]
public class FSMListReferenceEventPair
{
    [TableColumnWidth(60)]
    public FiniteStateMachineReference fsmListReference;
    public string responseEvent;
}
