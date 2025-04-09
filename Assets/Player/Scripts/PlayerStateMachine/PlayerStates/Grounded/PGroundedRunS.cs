using UnityEngine;

public class PGroundedRunS : PGroundedSuperState
{
    public PGroundedRunS(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
        : base(player, rigidBody, playerInput, cameraRotation, stateManager) { }


    public override void OnEnter()
    {
        base.OnEnter();

        // Debug.Log("Run State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        DepleteStamina();


        // --- State Transitions ---
        if ((!_runInput || _Player.CurrentStamina <= 0.0f || _movementInput == Vector2.zero) && _didPhysicsUpdateRan)
        {
            _StateManager.ChangeState(_Player.GroundedWalkS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        CalculateSlopeAngle();

        _movementVector = ProcessMovementVector(_movementInput, _Player.runSpeed);

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
