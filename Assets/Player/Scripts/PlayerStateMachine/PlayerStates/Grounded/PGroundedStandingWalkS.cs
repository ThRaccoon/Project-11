using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PGroundedStandingWalkS : PGroundedSuperState
{
    public PGroundedStandingWalkS(Player player, PlayerStateManager stateManager, bool didPhysicsUpdateRan)
        : base(player, stateManager, didPhysicsUpdateRan) { }

    public override void OnEnter()
    {
        base.OnEnter();

        _player.currentPlayerScale = PlayerScale.Standing;

        //Debug.Log("Standing Walk");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_movementInput == Vector2.zero && _didPhysicsUpdateRan)
        {
            _stateMachine.ChangeState(_player.GroundedStandingIdleS);
        }

        if (_crouchInput && _didPhysicsUpdateRan)
        {
            _stateMachine.ChangeState(_player.GroundedCrouchingWalkS);
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

        _player.CalculateSlopeAngle();

        _movementVector = _player.ProcessMovementVector(_movementInput, _player.walkSpeed);

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
    }
}
