using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PGroundedWalkS : PGroundedSuperState
{
    public PGroundedWalkS(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateMachine) 
        : base(player, rigidBody, playerInput, cameraRotation, stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // Debug.Log("Walk State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------

      
        // --- State Transitions ---
        if (_movementInput == Vector2.zero && _didPhysicsUpdateRan)
        {
            _StateMachine.ChangeState(_Player.GroundedIdleS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        CalculateSlopeAngle();

        //_movementVector = _player.ProcessMovementVector(_movementInput, _player.walkSpeed);
        _movementVector = ProcessMovementVector(_movementInput, _Player.walkSpeed);


        if (_slopeAngle > 0)
        {
            _movementVector = CalculateSlopeProjection(_movementVector);
        }

        ApplyMovementForce(_movementVector);

        _didPhysicsUpdateRan = true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
