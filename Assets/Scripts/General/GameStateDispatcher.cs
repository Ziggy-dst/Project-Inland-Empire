using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityAtoms.FSM;

public class GameStateDispatcher : MonoBehaviour
{
    [SerializeField] private FiniteStateMachineReference gameStateMachineReference, winningStateMachineReference;

    public void DispatchGameOverIfDead(IntReference health)
    {
        if (health.Value <= 0)
        {
            gameStateMachineReference.Machine.Dispatch("SetGameOver");
            winningStateMachineReference.Machine.Dispatch("SetPCPlayerWins");
        }
    }
    
    public void DispatchGameOverIfTimeUp(IntReference time)
    {
        if (time.Value <= 0)
        {
            gameStateMachineReference.Machine.Dispatch("SetGameOver");
            winningStateMachineReference.Machine.Dispatch("SetPCPlayerWins");
        }
    }
    
    public void DispatchGameOverIfTaskComplete(IntReference taskNum)
    {
        if (taskNum.Value >= 3)
        {
            gameStateMachineReference.Machine.Dispatch("SetGameOver");
            winningStateMachineReference.Machine.Dispatch("SetVRPlayerWins");
        }
    }
}
