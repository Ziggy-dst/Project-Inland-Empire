using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.FSM;

public class GameStateDispatcher : MonoBehaviour
{
    [SerializeField] private FiniteStateMachineReference gameStateMachineReference, winningStateMachineReference;

    public void DispatchGameOverIfDead(int health)
    {
        if (health <= 0)
        {
            gameStateMachineReference.Machine.Dispatch("SetGameOver");
            winningStateMachineReference.Machine.Dispatch("SetPCPlayerWins");
        }
    }
    
    public void DispatchGameOverIfTimeUp(int time)
    {
        if (time <= 0)
        {
            gameStateMachineReference.Machine.Dispatch("SetGameOver");
            winningStateMachineReference.Machine.Dispatch("SetPCPlayerWins");
        }
    }
    
    public void DispatchGameOverIfTaskComplete(int taskNum)
    {
        if (taskNum >= 3)
        {
            gameStateMachineReference.Machine.Dispatch("SetGameOver");
            winningStateMachineReference.Machine.Dispatch("SetVRPlayerWins");
        }
    }
}
