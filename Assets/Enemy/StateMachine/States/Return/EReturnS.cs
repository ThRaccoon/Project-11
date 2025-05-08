using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Return", menuName = "Enemy States/Return/Return")]
public class EReturnS : EReturnSuperS
{
    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, EnemyStateManager stateManager, Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, stateManager, playerTransform);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        Debug.Log("Enemy Return State");

        ToggleRigWeight(false);
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