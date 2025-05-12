using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EChaseSuperS : EnemyBaseSuperState
{
    protected GlobalTimer _avoidancePriorityTimer;

    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, AnimationManager animationManager, Rig rig, EnemyStateManager stateManager, 
        Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, animationManager, rig, stateManager, playerTransform);

        _avoidancePriorityTimer = new GlobalTimer(_enemy.avoidancePriorityDuration);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        _avoidancePriorityTimer.CountDownTimer();

        if (_avoidancePriorityTimer.Flag)
        {
            _navMeshAgent.avoidancePriority = Random.Range(0, 100);

            _avoidancePriorityTimer.Reset();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- Logic ---
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_enemy.ShouldAttack && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.attackStateController);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();

        if (IsPlayerInRange(_enemy.attackRange))
        {
            if (IsPlayerInLOS())
            {
                _enemy.ShouldAttack = true;
            }
        }

        _didPhysicsUpdateRan = true;
    }

    public override void DoOnExit()
    {
        base.DoOnExit();

        _avoidancePriorityTimer.Reset();
    }
}