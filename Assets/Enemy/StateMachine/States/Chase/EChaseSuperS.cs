using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EChaseSuperS : EnemyBaseSuperState
{
    protected NavMeshPath _navMeshPath;


    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, Transform playerTransform, EnemyStateManager stateManager)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, playerTransform, stateManager);

        _navMeshPath = new NavMeshPath();
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        if (NullChecker.Check(_navMeshAgent))
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
            _navMeshAgent.CalculatePath(_playerTransform.transform.position, _navMeshPath);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        if (IsPlayerInAttackRange() && _navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            _stateManager.ChangeState(_enemy.attackStateController);
        }

        if (_navMeshPath.status == NavMeshPathStatus.PathPartial)
        {
            _stateManager.ChangeState(_enemy.idleStateController);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    public override void DoOnExit()
    {
        base.DoOnExit();

        if (NullChecker.Check(_navMeshAgent))
        {
            _navMeshAgent.ResetPath();
        }
    }


    protected bool IsPlayerInAttackRange()
    {
        float _distanceToPlayer = Vector3.Distance(_enemyTransform.position, _playerTransform.position);

        if (_distanceToPlayer <= _enemy.attackRange)
        {
            return true;
        }
        return false;
    }
}