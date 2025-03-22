using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLandedS : PGroundedSuperState
{
    public PLandedS(Player player, PlayerStateManager stateManager, bool didPhysicsUpdateRan)
        : base(player, stateManager, didPhysicsUpdateRan) { }

    public override void OnEnter()
    {
        base.OnEnter();

        //Debug.Log("Landed");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_player.previousPlayerScale == PlayerScale.Standing)
        {
            _stateMachine.ChangeState(_player.GroundedStandingWalkS);
        }
        else
        {
            _stateMachine.ChangeState(_player.GroundedCrouchingWalkS);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        _player.ResetPullDownForce();
    }
}
