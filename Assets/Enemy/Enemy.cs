using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rig _rig;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("Ranges")]
    [field: SerializeField] public float chaseRange { get; private set; }
    [field: SerializeField] public float attackRange { get; private set; }

    [field: Space(10)]
    [field: Header("Speed")]
    [field: SerializeField] public float walkSpeed { get; private set; }
    [field: SerializeField] public float chaseSpeed { get; private set; }

    [field: Space(10)]
    [field: Header("Timers")]
    [field: SerializeField] public float avoidancePriorityDuration { get; private set; }
    [field: SerializeField] public float recalcPathDuration { get; private set; }
    [field: SerializeField] public Vector2 waitBeforeGiveUpDuration { get; private set; }

    [field: Space(10)]
    [field: Header("Other")]
    [field: SerializeField] public float transformYOffset;
    [field: SerializeField] public LayerMask collisionLayersToIgnore { get; private set; }
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("Patrol Settings")]
    [field: SerializeField] public float waitOnPatrolPointDuration { get; private set; }
    [field: SerializeField] public Transform patrolPointA { get; private set; }
    [field: SerializeField] public Transform patrolPointB { get; private set; }
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Public Variables ---
    [field: Space(30)]
    [field: Header("Debug")]
    [field: SerializeField] public Vector3 spawnPos { get; private set; }

    // --- Private Variables ---
    private AnimationManager _animationManager;
    private Transform _playerTransform;

    private bool _shouldChase;
    #region Getters / Setters

    public bool ShouldChase
    {
        get => _shouldChase;
        set => _shouldChase = value;
    }
    #endregion

    private bool _shouldAttack;
    #region Getters / Setters

    public bool ShouldAttack
    {
        get => _shouldAttack;
        set => _shouldAttack = value;
    }
    #endregion


    #region State Machine
    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("State Templates")]
    [SerializeField] private EIdleSuperS idleSTemplate;
    [SerializeField] private EChaseSuperS chaseSTemplate;
    [SerializeField] private EAttackSuperS attackSTemplate;
    [SerializeField] private EReturnS returnSTemplate;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // --- Scriptable Object State Instances ---
    public EIdleSuperS idleSInstance { get; private set; }
    public EChaseSuperS chaseSInstance { get; private set; }
    public EAttackSuperS attackSInstance { get; private set; }
    public EReturnSuperS returnSInstance { get; private set; }

    // --- State Manager --- 
    public EnemyStateManager stateManager { get; private set; }

    // --- State Controllers ---
    public EnemyBaseStateController baseStateController { get; private set; }
    public EIdleSController idleStateController { get; private set; }
    public EChaseSController chaseStateController { get; private set; }
    public EAttackSController attackStateController { get; private set; }
    public EReturnSController returnStateController { get; private set; }
    #endregion

    private void Awake()
    {
        // --- Components ---
        _boxCollider = GetComponent<BoxCollider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _animationManager = new AnimationManager(_animator);
        _rig = GetComponentInChildren<Rig>();

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // --- Assigned On Start ---
        attackRange += _navMeshAgent.stoppingDistance;

        // --- Instantiate State Instances ---
        idleSInstance = Instantiate(idleSTemplate);
        chaseSInstance = Instantiate(chaseSTemplate);
        attackSInstance = Instantiate(attackSTemplate);
        returnSInstance = Instantiate(returnSTemplate);

        // --- State Machine --- 
        stateManager = new EnemyStateManager();

        // --- State Controllers ---
        baseStateController = new EnemyBaseStateController(this);
        idleStateController = new EIdleSController(this);
        chaseStateController = new EChaseSController(this);
        attackStateController = new EAttackSController(this);
        returnStateController = new EReturnSController(this);
    }

    private void Start()
    {
        spawnPos = transform.position;

        if (Util.IsNotNull(_navMeshAgent) && Util.IsNotNull(_animator) && Util.IsNotNull(_rig))
        {
            idleSInstance.Initialize(this, transform, _navMeshAgent, _animator, _animationManager, _rig, stateManager, _playerTransform);
            chaseSInstance.Initialize(this, transform, _navMeshAgent, _animator, _animationManager, _rig, stateManager, _playerTransform);
            attackSInstance.Initialize(this, transform, _navMeshAgent, _animator, _animationManager, _rig, stateManager, _playerTransform);
            returnSInstance.Initialize(this, transform, _navMeshAgent, _animator, _animationManager, _rig, stateManager, _playerTransform);
        }

        stateManager.Initialize(idleStateController);
    }

    private void Update()
    {
        stateManager.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateManager.currentState.PhysicsUpdate();
    }


    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}