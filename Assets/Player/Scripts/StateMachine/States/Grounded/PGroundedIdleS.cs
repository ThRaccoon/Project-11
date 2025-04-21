using UnityEngine;

public class PGroundedIdleS : PGroundedSuperS
{
    public PGroundedIdleS(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
        : base(player, rigidBody, playerInput, cameraRotation, stateManager) { }


    public override void OnEnter()
    {
        base.OnEnter();

        // Debug.Log("Idle State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        if (_movementInput != Vector2.zero && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_player.groundedWalkS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        ApplyStoppingForce();

        _didPhysicsUpdateRan = true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}