using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using Sirenix.OdinInspector;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private StringVariable currentRoom;

    [TableList]
    [SerializeField] private List<AmbientDictionary> Ambients = new List<AmbientDictionary>();

    public void SwitchAmbient()
    {
        print("stop ambient");
        foreach (var pair in Ambients)
        {
            if (currentRoom.Value.Equals("Default")) pair.ambient.Stop();
            else
            {
                if (currentRoom.Value.Equals(pair.roomName))
                {
                    print("play ambient");
                    pair.ambient.Play();
                }
            }
        }
    }
}

[Serializable]
public class AmbientDictionary
{
    [TableColumnWidth(60)]
    public string roomName;
    public AudioSource ambient;
}
