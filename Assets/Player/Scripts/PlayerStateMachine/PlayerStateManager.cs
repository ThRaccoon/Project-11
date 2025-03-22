using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager 
{ 
    public PlayerBaseState CurrentState {  get; private set; }  


    public void Initialize(PlayerBaseState startingState) 
    {
        CurrentState = startingState;
        CurrentState.OnEnter();
    }

    public void ChangeState(PlayerBaseState newState) 
    {
        CurrentState.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter(); 
    }
}
