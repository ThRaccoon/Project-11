using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Attack", menuName = "Enemy States/Attack/Attack")]
public class EAttackS : EAttackSuperState
{
    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, Transform playerTransform, EnemyStateManager stateManager)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, playerTransform, stateManager);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        ToggleRigWeight(true);

        Debug.Log("Enemy Attack State");
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