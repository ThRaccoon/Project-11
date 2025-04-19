using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EIdleSuperS : EnemyBaseSuperState
{
    protected bool _shouldAttack = false;


    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, Transform playerTransform, EnemyStateManager stateManager)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, rig, playerTransform, stateManager);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        PlayAnimation("Idle");

        _shouldAttack = false;
    }

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        if (_shouldAttack && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.chaseStateController);
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();

        if (CheckIfPlayerInRange())
        {
            if (CheckIfPlayerInLOS())
            {
                _shouldAttack = true;
            }
        }

        _didPhysicsUpdateRan = true;
    }

    public override void DoOnExit()
    {
        base.DoOnExit();
    }


    protected bool CheckIfPlayerInRange()
    {
        float _distanceToPlayer = Vector3.Distance(_enemyTransform.position, _playerTransform.position);

        if (_distanceToPlayer < _enemy.chaseRange)
        {
            return true;
        }
        return false;
    }

    protected bool CheckIfPlayerInLOS()
    {
        Vector3 start = new Vector3(_enemyTransform.position.x,
                                    _enemyTransform.position.y + _enemy.transformYOffset,
                                    _enemyTransform.position.z);
        Vector3 end = _playerTransform.position;
        RaycastHit hit;

        Physics.Linecast(start, end, out hit, ~_enemy.collisionLayersToIgnore);
        if (hit.collider.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }
}