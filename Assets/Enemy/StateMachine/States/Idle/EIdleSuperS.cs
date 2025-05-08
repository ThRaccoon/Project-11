using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EIdleSuperS : EnemyBaseSuperState
{
    protected bool _shouldReturnToSpawn = false;
    protected GlobalTimer _returnToSpawnTimer;

    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, EnemyStateManager stateManager, Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, stateManager, playerTransform);

        _returnToSpawnTimer = new GlobalTimer(Random.Range(Mathf.RoundToInt(_enemy.returnToSpawnDuration.x),
                                                           Mathf.RoundToInt(_enemy.returnToSpawnDuration.y) + 1));
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        _navMeshAgent.CalculatePath(_enemy.spawnPos, _pathToSpawn);
        _navMeshAgent.CalculatePath(_playerTransform.position, _pathToPlayer);
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        if (_shouldReturnToSpawn)
        {
            _returnToSpawnTimer.CountDownTimer();
        }
        else
        {
            _returnToSpawnTimer.Reset();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        if (_returnToSpawnTimer.Flag)
        {
            _navMeshAgent.CalculatePath(_enemy.spawnPos, _pathToSpawn);
            _returnToSpawnTimer.Reset();

            _didLogicUpdateRan = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        if ((_shouldAttack || _enemy.PlayerAttacked) && _pathToPlayer.status == NavMeshPathStatus.PathComplete && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.chaseStateController);
        }

        if (!IsPositionReached(_enemyTransform.position, _enemy.spawnPos) && _pathToSpawn.status == NavMeshPathStatus.PathComplete
            && _shouldReturnToSpawn && _didLogicUpdateRan && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.returnStateController);
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

        _returnToSpawnTimer.Reset();
    }
}