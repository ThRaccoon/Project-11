using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Idle Patrol", menuName = "Enemy States/Idle/Patrol")]
public class EnemyIdlePatrolS : EIdleSuperS
{
    private bool _didIdleAnimWasPlayed = false;
    private bool _didWalkAnimWasPlayed = false;
    private Transform _currentPatrolPoint;
    private Transform _nextPatrolPoint;
    private NavMeshPath _patrolPath;
    private GlobalTimer _waitOnPatrolPointTimer;

    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, EnemyStateManager stateManager, Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, stateManager, playerTransform);

        _currentPatrolPoint = _enemy.patrolPointA;
        _nextPatrolPoint = _enemy.patrolPointB;

        _patrolPath = new NavMeshPath();

        _waitOnPatrolPointTimer = new GlobalTimer(_enemy.waitOnPatrolPointDuration);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        Debug.Log("Enemy Idle Patrol State");

        _shouldReturnToSpawn = false;

        _navMeshAgent.speed = _enemy.walkSpeed;

        ToggleRigWeight(false);
        PlayAnimation("Idle");
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        _recalculatePathTimer.CountDownTimer();
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        if (!IsPositionReached(_enemyTransform.position, _currentPatrolPoint.position))
        {
            if (_recalculatePathTimer.Flag)
            {
                _navMeshAgent.CalculatePath(_currentPatrolPoint.position, _patrolPath);

                if (_patrolPath.status == NavMeshPathStatus.PathComplete)
                {
                    _navMeshAgent.SetDestination(_currentPatrolPoint.position);

                    PlayWalkAnimIfNeeded();

                    _shouldReturnToSpawn = false;
                }
                else
                {
                    _navMeshAgent.ResetPath();

                    PlayIdleAnimIfNeeded();

                    _shouldReturnToSpawn = true;
                }

                _recalculatePathTimer.Reset();
            }
        }
        else
        {
            _waitOnPatrolPointTimer.CountDownTimer();

            if (_waitOnPatrolPointTimer.Flag)
            {
                SwapPatrolPoints();

                _waitOnPatrolPointTimer.Reset();
            }

            PlayIdleAnimIfNeeded();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    public override void DoOnExit()
    {
        base.DoOnExit();

        _waitOnPatrolPointTimer.Reset();
        _recalculatePathTimer.Reset();
    }


    private void SwapPatrolPoints()
    {
        Transform tempNavPoint = _currentPatrolPoint;
        _currentPatrolPoint = _nextPatrolPoint;
        _nextPatrolPoint = tempNavPoint;
    }

    private void PlayIdleAnimIfNeeded()
    {
        if (_didIdleAnimWasPlayed)
            return;

        _didIdleAnimWasPlayed = true;
        _didWalkAnimWasPlayed = false;

        PlayAnimation("Idle");
    }

    private void PlayWalkAnimIfNeeded()
    {
        if (_didWalkAnimWasPlayed)
            return;

        _didIdleAnimWasPlayed = false;
        _didWalkAnimWasPlayed = true;

        PlayAnimation("Walk");
    }
}