using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFallingS : PFallingSuperState
{
    public PFallingS(Player player, PlayerStateManager stateMachine, bool didPhysicsUpdateRan) : base(player, stateMachine, didPhysicsUpdateRan) { }

    public override void OnEnter()
    {
        base.OnEnter();

        //Debug.Log("Falling");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_player.previousPlayerScale == PlayerScale.Standing)
        {
            _stateMachine.ChangeState(_player.FallingStandingS);
        }
        else
        {
            _stateMachine.ChangeState(_player.FallingCrouchingS);
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
    }
}
