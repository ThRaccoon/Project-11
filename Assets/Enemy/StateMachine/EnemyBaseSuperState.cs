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

    protected Transform _playerTransform;

    protected EnemyStateManager _stateManager;

    // --- variables ---
    protected bool _didPhysicsUpdateRan = false;


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
        _didPhysicsUpdateRan = false;
    }

    public virtual void DoLogicUpdate() { }

    public virtual void DoPhysicsUpdate() { }

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
}