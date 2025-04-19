using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Idle Stationery", menuName = "Enemy States/Idle/Stationery")]
public class EIdleStationeryS : EIdleSuperS
{
    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, Transform playerTransform, EnemyStateManager stateManager)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, playerTransform, stateManager);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        ToggleRigWeight(true);

        Debug.Log("Enemy Idle Stationery State");
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