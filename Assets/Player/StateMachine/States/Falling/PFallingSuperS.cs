using UnityEngine;

public class PFallingSuperS : PlayerBaseState
{
    public PFallingSuperS(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
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

        // --- Logic ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        if (_player.IsGrounded && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_player.landedS);
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


    protected float CalculatePullDownForce()
    {
        if (_player.CurrentPullDownForce == _player.maxPullDownForce)
        {
            return _player.maxPullDownForce;
        }

        _player.forceIncrementTimer.Tick();

        if (_player.forceIncrementTimer.Flag)
        {
            _player.AccumulatedForceValue += _player.pullDownForceIncrement;
            _player.forceIncrementTimer.Reset();
        }

        _player.CurrentPullDownForce = _player.AccumulatedForceValue + _player.minPullDownForce;

        if (_player.CurrentPullDownForce < _player.maxPullDownForce)
        {
            _player.CurrentPullDownForce = _player.maxPullDownForce;
        }

        return _player.CurrentPullDownForce;
    }
}