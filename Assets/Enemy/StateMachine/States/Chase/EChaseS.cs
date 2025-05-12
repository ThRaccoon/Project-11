using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Chase", menuName = "Enemy States/Chase/Chase")]
public class EChaseS : EChaseSuperS
{
    private GlobalTimer _waitBeforeGiveUpTimer;

    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, AnimationManager animationManager, Rig rig, EnemyStateManager stateManager,
        Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, animationManager, rig, stateManager, playerTransform);

        _waitBeforeGiveUpTimer = new GlobalTimer(Random.Range(Mathf.RoundToInt(_enemy.waitBeforeDuration.x),
                                                              Mathf.RoundToInt(_enemy.waitBeforeDuration.y) + 1));
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
        _recalculatePathTimer.CountDownTimer();

        if (_recalculatePathTimer.Flag)
        {
            _navMeshAgent.CalculatePath(_playerTransform.position, _pathToPlayer);

            _recalculatePathTimer.Reset();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- Logic ---
        if (_pathToPlayer.status == NavMeshPathStatus.PathComplete)
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
            _animationManager.PlayAnim("Chase");


            _waitBeforeGiveUpTimer.Reset();
        }
        else
        {
            _animationManager.PlayAnim("Idle");
            _navMeshAgent.ResetPath();


            _waitBeforeGiveUpTimer.CountDownTimer();
        }

        OnAnimatorMove();
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

        _waitBeforeGiveUpTimer.Reset();
        _recalculatePathTimer.Reset();

        _navMeshAgent.ResetPath();
    }
}