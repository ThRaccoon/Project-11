using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Chase", menuName = "Enemy States/Chase/Chase")]
public class EChaseS : EChaseSuperState
{
    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, Transform playerTransform, EnemyStateManager stateManager)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, playerTransform, stateManager);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        ToggleRigWeight(true);

        Debug.Log("Enemy Chase State");
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
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