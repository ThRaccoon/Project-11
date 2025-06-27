using UnityEngine;

public class PGroundedSuperS : PlayerBaseState
{
    public PGroundedSuperS(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
        : base(player, rigidBody, playerInput, cameraRotation, stateManager) { }


    public override void OnEnter()
    {
        base.OnEnter();

        // Debug.Log("Grounded Super State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        if (!_player.IsGrounded && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_player.airborneS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        IsGrounded(_player.sphereCastLength);

        _didPhysicsUpdateRan = true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected void ApplyStoppingForce()
    {
        _rigidbody.linearVelocity *= _player.stoppingForce;

        if (_rigidbody.linearVelocity.magnitude < 0.01f)
        {
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }

    protected void ResetPullDownForce()
    {
        _player.AccumulatedForceValue = 0;
        _player.CurrentPullDownForce = _player.defaultPullDownForce;
        _player.forceIncrementTimer.Reset();
    }
}