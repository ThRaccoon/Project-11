using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGroundedSuperState : PlayerBaseState
{
    protected float _slopeAngle = 0.0f;


    public PGroundedSuperState(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
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


        // --- State Transitions ---
        if (!_Player.IsGrounded && _didPhysicsUpdateRan)
        {
            _StateManager.ChangeState(_Player.AirborneS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        IsGrounded(_Player.standingGroundCheckLength);

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
        if (_Rigidbody != null)
        {
            _Rigidbody.linearVelocity *= _Player.stoppingForce;
            if (_Rigidbody.linearVelocity.magnitude < 0.01f)
            {
                _Rigidbody.linearVelocity = Vector3.zero;
            }
        }
    }

    protected void ResetPullDownForce()
    {
        _Player.AccumulatedForceValue = 0;
        _Player.CurrentPullDownForce = _Player.defaultPullDownForce;
        _Player.forceIncrementTimer.Reset();
    }
}
