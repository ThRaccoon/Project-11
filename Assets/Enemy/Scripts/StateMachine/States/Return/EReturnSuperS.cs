using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EReturnSuperS : EnemyBaseSuperState
{
    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, AnimationManager animationManager, Rig rig, EnemyStateManager stateManager,
        Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, animationManager, rig, stateManager, playerTransform);
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
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_enemy.ShouldAttack && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.attackStateController);
        }

        if (_enemy.ShouldChase && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.chaseStateController);
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

        if (IsPlayerInRange(_enemy.chaseRange))
        {
            if (IsPlayerInLOS())
            {
                _enemy.ShouldChase = true;
            }
        }

        _didPhysicsUpdateRan = true;
    }

    public override void DoOnExit()
    {
        base.DoOnExit();
    }
}