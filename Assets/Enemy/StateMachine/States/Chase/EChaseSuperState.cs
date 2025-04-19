using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EChaseSuperState : ScriptableObject
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
    protected NavMeshPath _navMeshPath;


    public virtual void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, Rig rig, Transform playerTransform, EnemyStateManager stateManager)
    {
        _enemy = enemy;
        _enemyTransform = enemyTransform;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _rig = rig;

        _playerTransform = playerTransform;

        _stateManager = stateManager;

        _navMeshPath = new NavMeshPath();
    }

    public virtual void DoOnEnter()
    {
        PlayAnimation("Chase");

        _didPhysicsUpdateRan = false;
    }

    public virtual void DoLogicUpdate()
    {
        _navMeshAgent.SetDestination(_playerTransform.position);

        if (IsPlayerInAttackRange())
        {
            _stateManager.ChangeState(_enemy.attackStateController);
        }

        _navMeshAgent.CalculatePath(_playerTransform.transform.position, _navMeshPath);
        if (_navMeshPath.status == NavMeshPathStatus.PathPartial)
        {
            _stateManager.ChangeState(_enemy.idleStateController);
        }
    }

    public virtual void DoPhysicsUpdate() { }

    public virtual void DoOnExit()
    {
        _navMeshAgent.ResetPath();
    }


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

    protected bool IsPlayerInAttackRange()
    {
        float _distanceToPlayer = Vector3.Distance(_enemyTransform.position, _playerTransform.position);

        if (_distanceToPlayer <= _enemy.attackRange)
        {
            return true;
        }
        return false;
    }
}