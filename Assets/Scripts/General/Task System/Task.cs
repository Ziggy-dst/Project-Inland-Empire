using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class Task : MonoBehaviour
{
    [SerializeField] private IntReference currentFinishedTaskNumber;
    [SerializeField] private string taskObjectName;

    private void OnTriggerEnter(Collider other)
    {
        if (taskObjectName.Equals(other.name)) currentFinishedTaskNumber.Value++;
    }
}
