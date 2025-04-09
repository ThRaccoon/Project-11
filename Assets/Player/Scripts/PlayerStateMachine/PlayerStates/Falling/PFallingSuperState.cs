using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFallingSuperState : PlayerBaseState
{
    public PFallingSuperState(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
        : base(player, rigidBody, playerInput, cameraRotation, stateManager) { }


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
            _StateManager.ChangeState(_Player.LandedS);
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
        if (_Player.CurrentPullDownForce == _Player.maxPullDownForce)
        {
            return _Player.maxPullDownForce;
        }

        _Player.forceIncrementTimer.CountDownTimer();

        if (_Player.forceIncrementTimer.flag)
        {
            _Player.AccumulatedForceValue += _Player.pullDownForceIncrement;
            _Player.forceIncrementTimer.Reset();
        }

        _Player.CurrentPullDownForce = _Player.AccumulatedForceValue + _Player.minPullDownForce;

        if (_Player.CurrentPullDownForce < _Player.maxPullDownForce) 
        {
            _Player.CurrentPullDownForce = _Player.maxPullDownForce;
        }

        return _Player.CurrentPullDownForce;
    }
}

