using UnityEngine;

public class PGroundedRunS : PGroundedSuperS
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

        // --- Logic ---
        DepleteStamina();
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        if ((!_runInput || _player.CurrentStamina <= 0.0f || _movementInput == Vector2.zero) && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_player.groundedWalkS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _movementVector = ProcessMovementVector(_movementInput, _player.runSpeed);

        ApplyMovementForce(_movementVector);

        _didPhysicsUpdateRan = true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}