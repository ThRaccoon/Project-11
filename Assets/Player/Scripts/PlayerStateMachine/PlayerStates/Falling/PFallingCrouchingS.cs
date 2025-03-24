using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFallingCrouchingS : PFallingSuperState
{
    public PFallingCrouchingS(Player player, PlayerStateManager stateManager, bool didPhysicsUpdateRan)
        : base(player, stateManager, didPhysicsUpdateRan) { }

    public override void OnEnter()
    {
        base.OnEnter();

        _player.currentPlayerScale = PlayerScale.Crouching;

        //Debug.Log("Falling Crouching");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (!_crouchInput && _player.canStandUp && _didPhysicsUpdateRan)
        {
            _stateMachine.ChangeState(_player.FallingStandingS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (_player.previousPlayerScale == PlayerScale.Standing)
        {
            _player.ApplyScale(false);
            _player.previousPlayerScale = PlayerScale.Crouching;
        }

        _movementVector = _player.ProcessMovementVector(_movementInput, _player.crouchSpeed);
        _movementVector.y = _player.CalculatePullDownForce();

        _player.ApplyMovementForce(_movementVector);

        _didPhysicsUpdateRan = true;
    }

    public override void OnExit()
    {
        base.OnExit();

        _player.wasPreviousStateFalling = true;
    }
}
