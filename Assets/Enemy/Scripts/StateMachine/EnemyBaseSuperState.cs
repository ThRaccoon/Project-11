using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyBaseSuperState : ScriptableObject
{
    // --- Blackboard ---
    protected Enemy _enemy;
    protected Transform _enemyTransform;
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected AnimationManager _animationManager;
    protected Rig _rig;
    protected EnemyStateManager _stateManager;

    protected Transform _playerTransform;

    // --- Other ---
    protected bool _didPhysicsUpdateRan = false;
    protected NavMeshPath _pathToPlayer;
    protected NavMeshPath _pathToSpawn;

    public virtual void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, AnimationManager animationManager, Rig rig, EnemyStateManager stateManager,
        Transform playerTransform)
    {
        // --- Blackboard ---
        _enemy = enemy;
        _enemyTransform = enemyTransform;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _animationManager = animationManager;
        _rig = rig;
        _stateManager = stateManager;

        _playerTransform = playerTransform;

        // --- Other ---
        _pathToPlayer = new NavMeshPath();
        _pathToSpawn = new NavMeshPath();
    }

    public virtual void DoOnEnter() { }

    public virtual void DoLogicUpdate() { }

    public virtual void DoPhysicsUpdate() { }

    public virtual void DoOnExit()
    {
        _didPhysicsUpdateRan = false;

        _enemy.ShouldChase = false;
        _enemy.ShouldAttack = false;
    }


    protected bool IsPlayerInRange(float range)
    {
        float _distanceToPlayer = Vector3.Distance(_enemyTransform.position, _playerTransform.position);

        if (_distanceToPlayer < range)
        {
            return true;
        }
        return false;
    }

    protected bool IsPlayerInLOS()
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

    protected bool IsOnPosition(Vector3 current, Vector3 end)
    {
        float distance = Vector3.Distance(current, end);

        if (distance < 1f)
        {
            return true;
        }
        return false;
    }

    protected void ToggleRigWeight(bool isActive)
    {
        _rig.weight = isActive ? 1 : 0;
    }

    protected void SetAgentSpeed(float speed)
    {
        _navMeshAgent.speed = speed;

        if (speed == 0f)
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.velocity = Vector3.zero;
            _navMeshAgent.isStopped = true;
        }
        else
        {
            _navMeshAgent.isStopped = false;
        }
    }
}