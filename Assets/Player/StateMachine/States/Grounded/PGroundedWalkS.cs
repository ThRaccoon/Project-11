using UnityEngine;

public class PGroundedWalkS : PGroundedSuperS
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

        // --- Logic ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        if (_movementInput == Vector2.zero && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_player.groundedIdleS);
        }

        if ((_runInput && _player.CurrentStamina > 0.0f && _movementInput != Vector2.zero) && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_player.groundedRunS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        CalculateSlopeAngle();

        _movementVector = ProcessMovementVector(_movementInput, _player.walkSpeed);

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