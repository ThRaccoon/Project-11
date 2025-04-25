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
    [field: Header("Settings")]
    [field: SerializeField] public float activationRange { get; private set; }
    [field: SerializeField] public float chaseRange { get; private set; }
    [field: SerializeField] public float attackRange { get; private set; }
    [field: SerializeField] public float transformYOffset;
    [field: SerializeField] public LayerMask collisionLayersToIgnore { get; private set; }
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("Patrol Settings")]
    [field: SerializeField] public float waitTime { get; private set; }
    [field: SerializeField] public Transform navPointA { get; private set; }
    [field: SerializeField] public Transform navPointB { get; private set; }
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Private Variables ---
    private Transform _playerTransform;


    #region State Machine
    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("State Templates")]
    [SerializeField] private EIdleSuperS idleSTemplate;
    [SerializeField] private EChaseSuperS chaseSTemplate;
    [SerializeField] private EAttackSuperS attackSTemplate;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // --- Scriptable Object State Instances ---
    public EIdleSuperS idleSInstance { get; private set; }
    public EChaseSuperS chaseSInstance { get; private set; }
    public EAttackSuperS attackSInstance { get; private set; }

    // --- State Manager --- 
    public EnemyStateManager stateManager { get; private set; }

    // --- State Controllers ---
    public EnemyBaseStateController baseStateController { get; private set; }
    public EIdleSController idleStateController { get; private set; }
    public EChaseSController chaseStateController { get; private set; }
    public EAttackSController attackStateController { get; private set; }
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
        if (Util.IsNotNull(_navMeshAgent) && Util.IsNotNull(_animator) && Util.IsNotNull(_rig))
        {
            idleSInstance.Initialize(this, transform, _navMeshAgent, _animator, _rig, _playerTransform, stateManager);
            chaseSInstance.Initialize(this, transform, _navMeshAgent, _animator, _rig, _playerTransform, stateManager);
            attackSInstance.Initialize(this, transform, _navMeshAgent, _animator, _rig, _playerTransform, stateManager);
        }

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

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, chaseRange);

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
