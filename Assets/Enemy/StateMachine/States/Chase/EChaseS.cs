using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Chase", menuName = "Enemy States/Chase/Chase")]
public class EChaseS : EChaseSuperS
{
    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, EnemyStateManager stateManager, Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, stateManager, playerTransform);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        Debug.Log("Enemy Chase State");

        ToggleRigWeight(true);
        PlayAnimation("Chase");
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