using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [Header("Auto Assigned")]
    [SerializeField] private BoxCollider _boxCollider = null;
    [SerializeField] private NavMeshAgent _navMeshAgent = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Rig _rig = null;
    [Header("----------")]
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("Settings")]
    // [field: SerializeField] public float activationRange { get; private set; } = 0.0f;
    [field: SerializeField] public float chaseRange { get; private set; } = 0.0f;
    [field: SerializeField] public float attackRange { get; private set; } = 0.0f;
    [field: SerializeField] public float transformYOffset = 0.0f;
    [field: SerializeField] public LayerMask collisionLayersToIgnore { get; private set; }
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("Patrol Settings")]
    [field: SerializeField] public float waitTime { get; private set; } = 0.0f;
    [field: SerializeField] public Transform navPointA { get; private set; } = null;
    [field: SerializeField] public Transform navPointB { get; private set; } = null;
    // ----------------------------------------------------------------------------------------------------------------------------------


    private Transform _playerTransform = null;


    #region State Machine
    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("State Templates")]
    [SerializeField] private EIdleSuperS idleSTemplate = null;
    [SerializeField] private EChaseSuperS chaseSTemplate = null;
    [SerializeField] private EAttackSuperS attackSTemplate = null;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // --- Scriptable Object State Instances ---
    public EIdleSuperS idleSInstance { get; private set; } = null;
    public EChaseSuperS chaseSInstance { get; private set; } = null;
    public EAttackSuperS attackSInstance { get; private set; } = null;

    // --- State Manager --- 
    public EnemyStateManager stateManager { get; private set; } = null;

    // --- State Controllers ---
    public EnemyBaseStateController baseStateController { get; private set; } = null;
    public EIdleSController idleStateController { get; private set; } = null;
    public EChaseSController chaseStateController { get; private set; } = null;
    public EAttackSController attackStateController { get; private set; } = null;
    #endregion

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


        // --- Instantiate State Instances ---
        idleSInstance = Instantiate(idleSTemplate);
        chaseSInstance = Instantiate(chaseSTemplate);
        attackSInstance = Instantiate(attackSTemplate);

        // --- State Machine --- 
        stateManager = new EnemyStateManager();

        // --- State Controllers ---
        baseStateController = new EnemyBaseStateController(this);
        idleStateController = new EIdleSController(this);
        chaseStateController = new EChaseSController(this);
        attackStateController = new EAttackSController(this);
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
        _navMeshAgent.avoidancePriority = Random.Range(0, 100);

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
