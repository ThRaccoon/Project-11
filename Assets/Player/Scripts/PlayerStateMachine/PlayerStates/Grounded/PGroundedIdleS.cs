using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGroundedIdleS : PGroundedSuperState
{
    public PGroundedIdleS(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateMachine)
        : base(player, rigidBody, playerInput, cameraRotation, stateMachine) { }


    public override void OnEnter()
    {
        base.OnEnter();

        // Debug.Log("Idle State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_movementInput != Vector2.zero && _didPhysicsUpdateRan)
        {
            _StateMachine.ChangeState(_Player.GroundedWalkS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        ApplyStoppingForce();

        _didPhysicsUpdateRan = true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
