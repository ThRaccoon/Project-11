using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EAttackSuperState : ScriptableObject
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
        PlayAnimation("Attack");

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