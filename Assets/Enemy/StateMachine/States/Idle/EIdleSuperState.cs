using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EIdleSuperState : ScriptableObject
{
    // --- Components ---
    protected Enemy _enemy = null;
    protected Transform _enemyTransform = null;
    protected NavMeshAgent _navMeshAgent = null;
    protected Animator _animator = null;
    protected Rig _rig = null;

    protected Transform _playerTransform = null;

    protected EnemyStateManager _stateManager = null;

    // --- variables ---
    protected bool _didPhysicsUpdateRan = false;
    protected bool _shouldAttack = false;


    public virtual void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, Transform playerTransform, EnemyStateManager stateManager)
    {
        _enemy = enemy;
        _enemyTransform = enemyTransform;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _rig = rig;

        _playerTransform = playerTransform;

        _stateManager = stateManager;
    }

    public virtual void DoOnEnter()
    {
        PlayAnimation("Idle");

        _shouldAttack = false;
        _didPhysicsUpdateRan = false;
    }

    public virtual void DoLogicUpdate()
    {
        if (_shouldAttack && _didPhysicsUpdateRan)
        {
            _stateManager.ChangeState(_enemy.chaseStateController);
        }
    }

    public virtual void DoPhysicsUpdate()
    {
        if (CheckIfPlayerInRange())
        {
            if (CheckIfPlayerInLOS())
            {
                _shouldAttack = true;
            }
        }

        _didPhysicsUpdateRan = true;
    }

    public virtual void DoOnExit() { }


    protected void ToggleRigWeight(bool isActive)
    {
        _rig.weight = isActive ? 1 : 0;
    }

    protected void PlayAnimation(string animationName, float blendValue = 0.2f)
    {
        if (_animator != null)
        {
            _animator.CrossFade(animationName, blendValue);
        }
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