using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityAtoms.FSM;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private StringVariable currentState = null;

    private void Start()
    {
        StateNameChanged(currentState.Value);
    }

    public void OnEventRaised(string stateName)
    {
        StateNameChanged(stateName);
    }

    private void StateNameChanged(string stateName)
    {
        switch (currentState.Value)
        {
            case "InGame":
                Time.timeScale = 1f;
                AudioListener.pause = false;
                break;
            case "GameOver":
                Time.timeScale = 0f;
                AudioListener.pause = true;
                break;
        }
    }
}
