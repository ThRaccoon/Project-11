using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGroundedCrouchingIdleS : PGroundedSuperState
{
    public PGroundedCrouchingIdleS(Player player, PlayerStateManager stateManager, bool didPhysicsUpdateRan)
        : base(player, stateManager, didPhysicsUpdateRan) { }

    public override void OnEnter()
    {
        base.OnEnter();

        _player.currentPlayerScale = PlayerScale.Crouching;

        if (_player.previousPlayerScale == PlayerScale.Standing)
        {
            _player.ApplyScale(false);
        }

        //Debug.Log("Crouching Idle");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_movementInput != Vector2.zero && _didPhysicsUpdateRan)
        {
            _stateMachine.ChangeState(_player.GroundedCrouchingWalkS);
        }

        if (!_crouchInput && _player.canStandUp && _didPhysicsUpdateRan)
        {
            _stateMachine.ChangeState(_player.GroundedStandingIdleS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _player.ApplyStoppingForce();

        _didPhysicsUpdateRan = true;
    }

    public override void OnExit()
    {
        base.OnExit();

        _player.previousPlayerScale = PlayerScale.Crouching;
    }
}
