using UnityEngine;

public class PGroundedSuperS : PlayerBaseState
{
    protected float _slopeAngle = 0.0f;


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
            stateManager.ChangeState(_player.airborneS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        IsGrounded(_player.standingGroundCheckLength);

        _didPhysicsUpdateRan = true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }


    protected void CalculateSlopeAngle()
    {
        _slopeAngle = Vector3.Angle(Vector3.up, _hitInfo.normal);
    }

    protected Vector3 CalculateSlopeProjection(Vector3 movementVector)
    {
        return Vector3.ProjectOnPlane(movementVector, _hitInfo.normal);
    }

    protected void ApplyStoppingForce()
    {
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity *= _player.stoppingForce;
            if (_rigidbody.linearVelocity.magnitude < 0.01f)
            {
                _rigidbody.linearVelocity = Vector3.zero;
            }
        }
    }

    protected void ResetPullDownForce()
    {
        _player.AccumulatedForceValue = 0;
        _player.CurrentPullDownForce = _player.defaultPullDownForce;
        _player.forceIncrementTimer.Reset();
    }
}