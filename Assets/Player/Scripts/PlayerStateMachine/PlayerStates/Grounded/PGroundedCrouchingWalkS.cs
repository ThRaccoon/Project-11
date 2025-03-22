using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGroundedCrouchingWalkS : PGroundedSuperState
{
    public PGroundedCrouchingWalkS(Player player, PlayerStateManager stateManager, bool didPhysicsUpdateRan)
        : base(player, stateManager, didPhysicsUpdateRan) { }

    public override void OnEnter()
    {
        base.OnEnter();

        _player.currentPlayerScale = PlayerScale.Crouching;

        if (_player.previousPlayerScale == PlayerScale.Standing)
        {
            _player.ApplyScale(false);
        }

        //Debug.Log("Crouching Walk");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_movementInput == Vector2.zero && _didPhysicsUpdateRan)
        {
            _stateMachine.ChangeState(_player.GroundedCrouchingIdleS);
        }

        if (!_crouchInput && _player.canStandUp && _didPhysicsUpdateRan)
        {
            _stateMachine.ChangeState(_player.GroundedStandingWalkS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _player.CalculateSlopeAngle();

        _movementVector = _player.ProcessMovementVector(_movementInput, _player.crouchSpeed);

        if (_player.slopeAngle > 0)
        {
            _movementVector = _player.CalculateSlopeProjection(_movementVector);
        }

        _player.ApplyMovementForce(_movementVector);

        _didPhysicsUpdateRan = true;
    }

    public override void OnExit()
    {
        base.OnExit();

        _player.previousPlayerScale = PlayerScale.Crouching;
    }
}