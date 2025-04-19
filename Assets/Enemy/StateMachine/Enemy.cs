using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [Header("Auto Assigned")]
    [SerializeField] private BoxCollider _boxCollider = null;
    [SerializeField] private NavMeshAgent _navMeshAgent = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Rig _rig = null;
    [Header("----------")]


    // [field: SerializeField] public float activationRange { get; private set; } = 0.0f;
    [field: SerializeField] public float chaseRange { get; private set; } = 0.0f;
    [field: SerializeField] public float attackRange { get; private set; } = 0.0f;

    [field: SerializeField] public float transformYOffset = 0.0f;

    private Transform _playerTransform = null;

    [field: SerializeField] public LayerMask collisionLayersToIgnore { get; private set; }


    // --- State Machine / States --- 
    public EnemyStateManager stateManager { get; private set; } = null;
    public EnemyBaseStateController baseStateController { get; private set; } = null;

    // --- States ---
    public EIdleStateController idleStateController { get; private set; } = null;
    public EChaseStateController chaseStateController { get; private set; } = null;
    public EAttackStateController attackStateController { get; private set; } = null;

    // --- Scriptable Objects States ---
    [field: Space(30)]
    [SerializeField] private EIdleSuperState idleSTemplate = null;
    [SerializeField] private EChaseSuperState chaseSTemplate = null;
    [SerializeField] private EAttackSuperState attackSTemplate = null;

    public EIdleSuperState idleSInstance { get; private set; } = null;
    public EChaseSuperState chaseSInstance { get; private set; } = null;
    public EAttackSuperState attackSInstance { get; private set; } = null;


    private void Awake()
    {
        // --- Components ---
        _boxCollider = GetComponent<BoxCollider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _rig = GetComponentInChildren<Rig>();

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // --- Assigned On Start ---
        attackRange += _navMeshAgent.stoppingDistance;

        idleSInstance = Instantiate(idleSTemplate);
        chaseSInstance = Instantiate(chaseSTemplate);
        attackSInstance = Instantiate(attackSTemplate);


        // --- State Machine / States --- 
        stateManager = new EnemyStateManager();
        baseStateController = new EnemyBaseStateController(this);

        // --- States ---
        idleStateController = new EIdleStateController(this);
        chaseStateController = new EChaseStateController(this);
        attackStateController = new EAttackStateController(this);
    }

    private void Start()
    {
        idleSInstance.Initialize(this, transform, _navMeshAgent, _animator, _rig, _playerTransform, stateManager);
        chaseSInstance.Initialize(this, transform, _navMeshAgent, _animator, _rig, _playerTransform, stateManager);
        attackSInstance.Initialize(this, transform, _navMeshAgent, _animator, _rig, _playerTransform, stateManager);

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
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, activationRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
