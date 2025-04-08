using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGroundedSuperState : PlayerBaseState
{
    protected float _slopeAngle = 0.0f;


    public PGroundedSuperState(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateMachine) 
        : base(player, rigidBody, playerInput, cameraRotation, stateMachine)
    {
    }

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
            _StateMachine.ChangeState(_Player.AirborneS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        IsGrounded(_Player.standingGroundCheckLength);
    }

    public override void OnExit()
    {
        base.OnExit();
    }


    // --- Slope Angle / Projection ---
    public void CalculateSlopeAngle()
    {
        _slopeAngle = Vector3.Angle(Vector3.up, _hitInfo.normal);
    }

    public Vector3 CalculateSlopeProjection(Vector3 movementVector)
    {
        return Vector3.ProjectOnPlane(movementVector, _hitInfo.normal);
    }


    // --- Apply Force ---
    public void ApplyStoppingForce()
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
}
