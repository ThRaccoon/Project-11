using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Attack", menuName = "Enemy States/Attack/Attack")]
public class EAttackS : EAttackSuperS
{
    public override void Initialize(Enemy enemy, Transform enemyTransform, NavMeshAgent navMeshAgent, Animator animator, AnimationManager animationManager, Rig rig, EnemyStateManager stateManager,
        Transform playerTransform)
    {
        base.Initialize(enemy, enemyTransform, navMeshAgent, animator, animationManager, rig, stateManager, playerTransform);
    }

    public override void DoOnEnter()
    {
        base.DoOnEnter();

        Debug.Log("Enemy Attack State");

        _enemyTransform.LookAt(new Vector3(_playerTransform.position.x,
                                           _enemyTransform.position.y,
                                           _playerTransform.position.z));


        //SetAgentSpeed(0f);

        //_navMeshAgent.ResetPath();
        //_navMeshAgent.speed = 0f;
        //_navMeshAgent.velocity = Vector3.zero;
        //_navMeshAgent.isStopped = true;

        _animationManager.PlayCrossFadeAnimation("Attack");



        ToggleRigWeight(true);
    }

    protected float a = 0.1f;

    public override void DoLogicUpdate()
    {
        base.DoLogicUpdate();

        // --- Timers ---
        a -= Time.deltaTime;
        if (a <= 0f)
        {
            _navMeshAgent.speed = 0f;
        }

        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
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
    }
}