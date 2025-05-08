using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EReturnSuperS : EnemyBaseSuperState
{
    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, EnemyStateManager stateManager, Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, stateManager, playerTransform);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        _navMeshAgent.speed = _enemy.walkSpeed;

        _navMeshAgent.SetDestination(_enemy.spawnPos);

        _navMeshAgent.CalculatePath(_enemy.spawnPos, _pathToSpawn);
        _navMeshAgent.CalculatePath(_playerTransform.position, _pathToPlayer);
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        _recalculatePathTimer.CountDownTimer();
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        if (_recalculatePathTimer.Flag)
        {
            _navMeshAgent.CalculatePath(_enemy.spawnPos, _pathToSpawn);

            if (_pathToSpawn.status == NavMeshPathStatus.PathComplete)
            {
                _navMeshAgent.SetDestination(_enemy.spawnPos);
            }

            _recalculatePathTimer.Reset();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        if ((_shouldAttack || _enemy.PlayerAttacked) && _pathToPlayer.status == NavMeshPathStatus.PathComplete && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.chaseStateController);
        }

        if (IsPositionReached(_enemyTransform.position, _enemy.spawnPos) && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.idleStateController);
        }

        if (!IsPositionReached(_enemyTransform.position, _enemy.spawnPos) && _pathToSpawn.status != NavMeshPathStatus.PathComplete && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.idleStateController);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();

        if (IsPlayerInRange(_enemy.chaseRange))
        {
            if (IsPlayerInLOS())
            {
                _navMeshAgent.CalculatePath(_playerTransform.position, _pathToPlayer);

                _shouldAttack = true;
            }
        }
        _didPhysicsUpdateRan = true;
    }

    public override void DoOnExit()
    {
        base.DoOnExit();

        _navMeshAgent.ResetPath();

        _recalculatePathTimer.Reset();
    }
}