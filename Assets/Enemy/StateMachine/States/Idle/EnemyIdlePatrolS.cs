using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Idle Patrol", menuName = "Enemy States/Idle/Patrol")]
public class EnemyIdlePatrolS : EIdleSuperS
{
    private bool _stopPatrolling = false;
    private Transform _currentNavPoint = null;
    private Transform _nextNavPoint = null;
    private GlobalTimer _timer;


    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, Transform playerTransform, EnemyStateManager stateManager)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, playerTransform, stateManager);

        _currentNavPoint = _enemy.navPointA;
        _nextNavPoint = _enemy.navPointB;
        _timer = new GlobalTimer(_enemy.waitTime);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        PlayAnimation("Chase");
        ToggleRigWeight(false);

        Debug.Log("Enemy Idle Patrol State");
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        if (!_stopPatrolling)
        {
            _navMeshAgent.SetDestination(_currentNavPoint.position);

            if (IsNavPointReached())
            {
                PlayAnimation("Idle");
                _timer.CountDownTimer();

                if (_timer.flag)
                {
                    // Should be walk
                    PlayAnimation("Chase");
                    SwapNavPoints();
                    _timer.Reset();
                }
            }
        }
        else 
        {
            PlayAnimation("Idle");
            ToggleRigWeight(true);
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

        _stopPatrolling = true;
    }


    private bool IsNavPointReached()
    {
        float distance = Vector3.Distance(_enemyTransform.position, _currentNavPoint.position);

        if (distance < 1.0f)
        {
            return true;
        }
        return false;
    }

    private void SwapNavPoints()
    {
        Transform tempNavPoint = _currentNavPoint;
        _currentNavPoint = _nextNavPoint;
        _nextNavPoint = tempNavPoint;
    }
}
