using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAirborneS : PFallingSuperState
{
    public PAirborneS(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
        : base(player, rigidBody, playerInput, cameraRotation, stateManager) { }


    public override void OnEnter()
    {
        base.OnEnter();

        // Debug.Log("Airborne State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        _StateManager.ChangeState(_Player.FallingS);
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
