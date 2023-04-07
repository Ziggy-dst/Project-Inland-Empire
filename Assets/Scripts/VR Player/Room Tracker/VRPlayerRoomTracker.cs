using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class VRPlayerRoomTracker : MonoBehaviour
{
    [SerializeField] private StringVariable currentRoom;

    // track music, statue
    private void OnTriggerEnter(Collider other)
    {
        currentRoom.Value = other.tag;
    }

    private void OnTriggerExit(Collider other)
    {
        currentRoom.Value = "default";
    }
}
