using UnityEngine;

public class PLandedS : PGroundedSuperState
{
    public PLandedS(Player player, Rigidbody rigidBody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
        : base(player, rigidBody, playerInput, cameraRotation, stateManager) { }


    public override void OnEnter()
    {
        base.OnEnter();

        // Debug.Log("Landed State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        stateManager.ChangeState(_player.groundedWalkS);
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        ResetPullDownForce();
    }
}