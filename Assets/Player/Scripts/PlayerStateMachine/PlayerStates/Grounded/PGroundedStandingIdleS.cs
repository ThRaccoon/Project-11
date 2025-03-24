using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGroundedStandingIdleS : PGroundedSuperState
{
    public PGroundedStandingIdleS(Player player, PlayerStateManager stateManager, bool didPhysicsUpdateRan)
        : base(player, stateManager, didPhysicsUpdateRan) { }

    public override void OnEnter()
    {
        base.OnEnter();

        _player.currentPlayerScale = PlayerScale.Standing;

        //Debug.Log("Standing Idle");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_movementInput != Vector2.zero && _didPhysicsUpdateRan)
        {
            _stateMachine.ChangeState(_player.GroundedStandingWalkS);
        }

        if (_crouchInput && _didPhysicsUpdateRan)
        {
            _stateMachine.ChangeState(_player.GroundedCrouchingIdleS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (_player.previousPlayerScale == PlayerScale.Crouching)
        {
            _player.ApplyScale(true);
            _player.previousPlayerScale = PlayerScale.Standing;
        }

        _player.ApplyStoppingForce();

        _didPhysicsUpdateRan = true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
