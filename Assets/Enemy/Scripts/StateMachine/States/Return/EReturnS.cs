using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Return", menuName = "Enemy States/Return/Return")]
public class EReturnS : EReturnSuperS
{
    private GlobalTimer _recalcPathToSpawnTimer;

    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, AnimationManager animationManager, Rig rig, EnemyStateManager stateManager,
        Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, animationManager, rig, stateManager, playerTransform);

        _recalcPathToSpawnTimer = new GlobalTimer(_enemy.recalcPathDuration);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        Debug.Log("Enemy Return State");

        _navMeshAgent.CalculatePath(_enemy.spawnPos, _pathToSpawn);

        ToggleRigWeight(true);
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        _recalcPathToSpawnTimer.Tick();

        if (_recalcPathToSpawnTimer.Flag)
        {
            _navMeshAgent.CalculatePath(_enemy.spawnPos, _pathToSpawn);

            _recalcPathToSpawnTimer.Reset();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- Logic ---
        if (_pathToSpawn.status == NavMeshPathStatus.PathComplete)
        {
            _navMeshAgent.SetDestination(_enemy.spawnPos);

            _enemy.SetAgentSpeed(_enemy.walkSpeed);
            _animationManager.PlayCrossFadeAnimation("Walk");
        }
        else
        {
            _enemy.SetAgentSpeed(0f);
            _animationManager.PlayCrossFadeAnimation("Idle");
        }
        // ----------------------------------------------------------------------------------------------------------------------------------


        // --- State Transitions ---
        if (IsOnPosition(_enemyTransform.position, _enemy.spawnPos) && _didPhysicsUpdateRan)
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

        _recalcPathToSpawnTimer.Reset();
    }
}