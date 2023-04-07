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
        // check if the finished task number has increased /TO DO
        if (taskObjectName.Equals(other.name))
        {
            print(taskObjectName);
            currentFinishedTaskNumber.Value++;
        }
    }
}
