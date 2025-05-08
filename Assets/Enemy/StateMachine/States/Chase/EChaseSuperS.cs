using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EChaseSuperS : EnemyBaseSuperState
{
    private GlobalTimer _avoidancePriorityTimer;

    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, EnemyStateManager stateManager, Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, stateManager, playerTransform);

        _avoidancePriorityTimer = new GlobalTimer(_enemy.avoidancePriorityDuration);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        _navMeshAgent.speed = _enemy.chaseSpeed;

        _navMeshAgent.SetDestination(_playerTransform.position);
        _navMeshAgent.CalculatePath(_playerTransform.transform.position, _pathToPlayer);
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        _avoidancePriorityTimer.CountDownTimer();
        _recalculatePathTimer.CountDownTimer();
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        if (_avoidancePriorityTimer.Flag)
        {
            _navMeshAgent.avoidancePriority = Random.Range(0, 100);
            _avoidancePriorityTimer.Reset();
        }

        if (_recalculatePathTimer.Flag)
        {
            _navMeshAgent.CalculatePath(_playerTransform.transform.position, _pathToPlayer);

            if (_pathToPlayer.status == NavMeshPathStatus.PathComplete)
            {
                _navMeshAgent.SetDestination(_playerTransform.position);
            }

            _recalculatePathTimer.Reset();
            _didLogicUpdateRan = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        //if (IsPlayerInRange(_enemy.attackRange) && IsPlayerInLOS())
        //{
        //    _stateManager.ChangeState(_enemy.attackStateController);
        //}

        if (_pathToPlayer.status != NavMeshPathStatus.PathComplete && _didLogicUpdateRan)
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

        _navMeshAgent.ResetPath();

        _avoidancePriorityTimer.Reset();
        _recalculatePathTimer.Reset();
    }
}