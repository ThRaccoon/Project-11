using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFallingSuperState : PlayerBaseState
{
    public PFallingSuperState(Player player, PlayerStateManager stateMachine, bool didPhysicsUpdateRan) : base(player, stateMachine, didPhysicsUpdateRan) { }

    public override void OnEnter()
    {
        base.OnEnter();

        //Debug.Log("Falling Super State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_player.isGrounded && _didPhysicsUpdateRan)
        {
            _stateMachine.ChangeState(_player.LandedS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (_player.currentPlayerScale == PlayerScale.Standing)
        {
            _player.IsGrounded(_player.standingGroundCheckLength);
        }
        else
        {
            _player.CanStandUp();
            _player.IsGrounded(_player.crouchingGroundCheckLength);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
