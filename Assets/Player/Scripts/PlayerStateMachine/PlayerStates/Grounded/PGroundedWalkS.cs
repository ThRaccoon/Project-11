using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGroundedWalkS : PGroundedSuperState
{
    public PGroundedWalkS(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
        : base(player, rigidBody, playerInput, cameraRotation, stateManager) { }


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
            _StateManager.ChangeState(_Player.GroundedIdleS);
        }
        
        if ((_runInput && _Player.CurrentStamina > 0.0f && _movementInput != Vector2.zero) && _didPhysicsUpdateRan) 
        {
            _StateManager.ChangeState(_Player.GroundedRunS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        CalculateSlopeAngle();

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
