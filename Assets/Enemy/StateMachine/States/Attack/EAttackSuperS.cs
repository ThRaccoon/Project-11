using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EAttackSuperS : EnemyBaseSuperState
{
    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, Transform playerTransform, EnemyStateManager stateManager)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, playerTransform, stateManager);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        PlayAnimation("Attack");
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    public override void DoOnExit()
    {
        base.DoOnExit();
    }
}