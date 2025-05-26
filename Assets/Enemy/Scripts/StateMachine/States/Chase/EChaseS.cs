using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Chase", menuName = "Enemy States/Chase/Chase")]
public class EChaseS : EChaseSuperS
{
    private GlobalTimer _recalcPathToPlayerTimer;
    private GlobalTimer _waitBeforeGiveUpTimer;

    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, AnimationManager animationManager, Rig rig, EnemyStateManager stateManager,
        Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, animationManager, rig, stateManager, playerTransform);

        _recalcPathToPlayerTimer = new GlobalTimer(_enemy.recalcPathDuration);
        _waitBeforeGiveUpTimer = new GlobalTimer(Random.Range(Mathf.RoundToInt(_enemy.waitBeforeGiveUpDuration.x),
                                                              Mathf.RoundToInt(_enemy.waitBeforeGiveUpDuration.y) + 1));
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        Debug.Log("Enemy Chase State");

        _navMeshAgent.CalculatePath(_playerTransform.position, _pathToPlayer);

        ToggleRigWeight(true);
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        _recalcPathToPlayerTimer.Tick();

        if (_recalcPathToPlayerTimer.Flag)
        {
            _navMeshAgent.CalculatePath(_playerTransform.position, _pathToPlayer);

            _recalcPathToPlayerTimer.Reset();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- Logic ---
        if (_pathToPlayer.status == NavMeshPathStatus.PathComplete)
        {
            _navMeshAgent.SetDestination(_playerTransform.position);

            _enemy.SetAgentSpeed(_enemy.chaseSpeed);
            _animationManager.PlayCrossFadeAnimation("Chase");

            _waitBeforeGiveUpTimer.Reset();
        }
        else
        {
            _enemy.SetAgentSpeed(0f);
            _animationManager.PlayCrossFadeAnimation("Idle");

            _waitBeforeGiveUpTimer.Tick();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (_waitBeforeGiveUpTimer.Flag && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.returnStateController);
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

        _recalcPathToPlayerTimer.Reset();
        _waitBeforeGiveUpTimer.Reset();
    }
}