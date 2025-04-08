using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFallingSuperState : PlayerBaseState
{
    public PFallingSuperState(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateMachine)
        : base(player, rigidBody, playerInput, cameraRotation, stateMachine) { }


    public override void OnEnter()
    {
        base.OnEnter();

        // Debug.Log("Falling Super State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_Player.IsGrounded && _didPhysicsUpdateRan)
        {
            _StateMachine.ChangeState(_Player.LandedS);
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


    protected float CalculatePullDownForce()
    {
        _Player.ForceIncrementTimer -= Time.fixedDeltaTime;
        _Player.CurrentPullDownForce = _Player.AccumulatedForceValue + _Player.minPullDownForce;

        if (_Player.ForceIncrementTimer < 0)
        {
            if (_Player.CurrentPullDownForce > _Player.maxPullDownForce)
            {
                _Player.AccumulatedForceValue += _Player.pullDownForceIncrement;
            }

            _Player.ForceIncrementTimer = _Player.forceIncrementTimeInterval;
        }
        return _Player.CurrentPullDownForce;
    }
}
