using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Idle Patrol", menuName = "Enemy States/Idle/Patrol")]
public class EnemyIdlePatrolS : EIdleSuperS
{
    private Transform _currentPatrolPoint;
    private Transform _nextPatrolPoint;
    private NavMeshPath _pathToPatrolPoint;
    private GlobalTimer _waitOnPatrolPointTimer;
    protected GlobalTimer _waitBeforeReturnToSpawnTimer;

    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, AnimationManager animationManager, Rig rig, EnemyStateManager stateManager,
        Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, animationManager, rig, stateManager, playerTransform);

        _currentPatrolPoint = _enemy.patrolPointA;
        _nextPatrolPoint = _enemy.patrolPointB;

        _pathToPatrolPoint = new NavMeshPath();

        _waitOnPatrolPointTimer = new GlobalTimer(_enemy.waitOnPatrolPointDuration);
        _waitBeforeReturnToSpawnTimer = new GlobalTimer(Random.Range(Mathf.RoundToInt(_enemy.waitBeforeDuration.x),
                                                                     Mathf.RoundToInt(_enemy.waitBeforeDuration.y) + 1));
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        Debug.Log("Enemy Idle Patrol State");

        _navMeshAgent.CalculatePath(_currentPatrolPoint.position, _pathToPatrolPoint);

        ToggleRigWeight(false);
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        _recalculatePathTimer.CountDownTimer();

        if (_recalculatePathTimer.Flag)
        {
            _navMeshAgent.CalculatePath(_currentPatrolPoint.position, _pathToPatrolPoint);

            _recalculatePathTimer.Reset();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- Logic ---
        if (IsOnPosition(_enemyTransform.position, _currentPatrolPoint.position))
        {
            _animationManager.PlayAnim("Idle");
            _navMeshAgent.ResetPath();


            _waitOnPatrolPointTimer.CountDownTimer();

            if (_waitOnPatrolPointTimer.Flag)
            {
                SwapPatrolPoints();

                _waitOnPatrolPointTimer.Reset();
            }
        }
        else
        {
            if (_pathToPatrolPoint.status == NavMeshPathStatus.PathComplete)
            {
                _navMeshAgent.SetDestination(_currentPatrolPoint.position);
                _animationManager.PlayAnim("Walk");


                _waitBeforeReturnToSpawnTimer.Reset();
            }
            else
            {
                _animationManager.PlayAnim("Idle");
                _navMeshAgent.ResetPath();


                _waitBeforeReturnToSpawnTimer.CountDownTimer();
            }
        }

        OnAnimatorMove();
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (!IsOnPosition(_enemyTransform.position, _enemy.spawnPos) && _waitBeforeReturnToSpawnTimer.Flag && _didPhysicsUpdateRan)
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

        _recalculatePathTimer.Reset();
        _waitOnPatrolPointTimer.Reset();

        _waitBeforeReturnToSpawnTimer.Duration = Random.Range(Mathf.RoundToInt(_enemy.waitBeforeDuration.x),
                                                              Mathf.RoundToInt(_enemy.waitBeforeDuration.y) + 1);
        _waitBeforeReturnToSpawnTimer.Reset();

        _navMeshAgent.ResetPath();
    }


    private void SwapPatrolPoints()
    {
        Transform tempNavPoint = _currentPatrolPoint;
        _currentPatrolPoint = _nextPatrolPoint;
        _nextPatrolPoint = tempNavPoint;
    }
}