using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyBaseSuperState : ScriptableObject
{
    // --- Components ---
    protected Enemy _enemy;
    protected Transform _enemyTransform;
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected Rig _rig;
    protected EnemyStateManager _stateManager;

    protected Transform _playerTransform;

    // --- variables ---
    protected bool _didLogicUpdateRan = false;
    protected bool _didPhysicsUpdateRan = false;
    protected bool _shouldAttack = false;
    protected NavMeshPath _pathToPlayer;
    protected NavMeshPath _pathToSpawn;
    protected GlobalTimer _recalculatePathTimer;

    public virtual void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, EnemyStateManager stateManager, Transform playerTransform)
    {
        _enemy = enemy;
        _enemyTransform = enemyTransform;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _rig = rig;
        _stateManager = stateManager;

        _playerTransform = playerTransform;

        _pathToPlayer = new NavMeshPath();
        _pathToSpawn = new NavMeshPath();

        _recalculatePathTimer = new GlobalTimer(_enemy.recalculatePathDuration);
    }

    public virtual void DoOnEnter() { }

    public virtual void DoLogicUpdate() { }

    public virtual void DoPhysicsUpdate() { }

    public virtual void DoOnExit()
    {
        _didLogicUpdateRan = false;
        _didPhysicsUpdateRan = false;

        _shouldAttack = false;
        _enemy.PlayerAttacked = false;
    }


    protected bool IsPositionReached(Vector3 current, Vector3 end)
    {
        float distance = Vector3.Distance(current, end);

        if (distance < 1.0f)
        {
            return true;
        }
        return false;
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

    protected void ToggleRigWeight(bool isActive)
    {
        _rig.weight = isActive ? 1 : 0;
    }

    protected void PlayAnimation(string animationName, float blendValue = 0.2f)
    {
        if (Util.IsNotNull(_animator))
        {
            _animator.CrossFade(animationName, blendValue);
        }
    }
}