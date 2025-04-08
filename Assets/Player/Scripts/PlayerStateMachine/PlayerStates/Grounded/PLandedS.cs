using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLandedS : PGroundedSuperState
{
    public PLandedS(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateMachine)
        : base(player, rigidBody, playerInput, cameraRotation, stateMachine) { }


    public override void OnEnter()
    {
        base.OnEnter();

        // Debug.Log("Landed State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        _StateMachine.ChangeState(_Player.GroundedWalkS);
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        ResetPullDownForce();
    }
}
