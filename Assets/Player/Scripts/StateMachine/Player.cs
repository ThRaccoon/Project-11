using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [Header("Auto Assigned")]
    [SerializeField] private CapsuleCollider _capsuleCollider = null;
    [SerializeField] private Rigidbody _rigidbody = null;
    [Header("----------")]
    [SerializeField] private Transform _cameraRotation = null;
    [SerializeField] private PlayerInput _playerInput = null;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("Walk")]
    [field: SerializeField] public float walkSpeed { get; private set; } = 8.0f;

    [field: Space(10)]
    [Tooltip("How fast the player stops. Value should be between 0 and 0.9.")]
    [field: SerializeField] public float stoppingForce { get; private set; } = 0.5f;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Header("Run")]
    [field: SerializeField] public float runSpeed { get; private set; } = 12.0f;

    [field: SerializeField] public float maxStamina { get; private set; } = 100.0f;
    [field: SerializeField] public float regenRate { get; private set; } = 10.0f;
    [field: SerializeField] public float depleteRate { get; private set; } = 30.0f;

    [field: SerializeField] public float staminaRegenDelay { get; private set; } = 3.0f;
    public GlobalTimer staminaRegenTimer { get; private set; } = null;


    #region Getters / Setters

    [SerializeField] private float _currentStamina = 0.0f;

    public float CurrentStamina
    {
        get => _currentStamina;
        set => _currentStamina = value;
    }
    #endregion
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [Header("Pull-Down Force Settings")]
    [field: SerializeField] public float defaultPullDownForce { get; private set; } = 0.0f;
    [field: SerializeField] public float minPullDownForce { get; private set; } = -65.0f;
    [field: SerializeField] public float maxPullDownForce { get; private set; } = -120.0f;

    [Tooltip("The incremental value added to the pull-down force over time.")]
    [field: SerializeField] public float pullDownForceIncrement { get; private set; } = -4.0f;

    [Tooltip("The time interval after which the pull-down force is incremented.")]
    [field: SerializeField] public float forceIncrementDelay { get; private set; } = 0.2f;
    public GlobalTimer forceIncrementTimer { get; private set; } = null;


    #region Getters / Setters

    [SerializeField] private float _currentPullDownForce = 0.0f;

    public float CurrentPullDownForce
    {
        get => _currentPullDownForce;
        set => _currentPullDownForce = value;
    }

    private float _accumulatedForceValue = 0.0f;

    public float AccumulatedForceValue
    {
        get => _accumulatedForceValue;
        set => _accumulatedForceValue = value;
    }
    #endregion
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("Ground / Ceiling  Checks")]
    [field: SerializeField] public float standingGroundCheckLength { get; private set; } = 0.125f;


    #region Getters / Setters
    private bool _isGrounded = false;

    public bool IsGrounded
    {
        get => _isGrounded;
        set => _isGrounded = value;
    }
    #endregion
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Drags ---
    private float _playerDrag = 4.0f;
    private float _playerAngularDrag = 4.0f;


    #region State Machine
    // --- State Manager --- 
    public PlayerStateManager stateManager { get; private set; } = null;

    // --- Base State ---
    public PlayerBaseState baseState { get; private set; } = null;

    // --- Grounded States ---
    public PGroundedSuperS groundedSuperState { get; private set; } = null;
    public PLandedS landedS { get; private set; } = null;
    public PGroundedIdleS groundedIdleS { get; private set; } = null;
    public PGroundedWalkS groundedWalkS { get; private set; } = null;
    public PGroundedRunS groundedRunS { get; private set; } = null;

    // --- Falling States ---
    public PFallingSuperS fallingSuperState { get; private set; } = null;
    public PAirborneS airborneS { get; private set; } = null;
    public PFallingS fallingS { get; private set; } = null;
    #endregion


    private void Awake()
    {
        // --- Components ---
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();

        // --- Assigned On Start ---
        _currentStamina = maxStamina;

        // --- Timers ---
        forceIncrementTimer = new GlobalTimer(forceIncrementDelay);
        staminaRegenTimer = new GlobalTimer(staminaRegenDelay);

        // --- Ray Casts ---
        if (NullChecker.Check(_capsuleCollider))
        {
            standingGroundCheckLength += _capsuleCollider.bounds.extents.y;
        }

        // --- Drags ---
        if (NullChecker.Check(_rigidbody))
        {
            _rigidbody.linearDamping = _playerDrag;
            _rigidbody.angularDamping = _playerAngularDrag;
        }

        if (NullChecker.Check(_rigidbody) && NullChecker.Check(_cameraRotation) && NullChecker.Check(_playerInput))
        {
            // --- State Machine / States --- 
            stateManager = new PlayerStateManager();
            baseState = new PlayerBaseState(this, _rigidbody, _playerInput, _cameraRotation, stateManager);

            // --- Grounded States ---
            groundedSuperState = new PGroundedSuperS(this, _rigidbody, _playerInput, _cameraRotation, stateManager);
            landedS = new PLandedS(this, _rigidbody, _playerInput, _cameraRotation, stateManager);
            groundedIdleS = new PGroundedIdleS(this, _rigidbody, _playerInput, _cameraRotation, stateManager);
            groundedWalkS = new PGroundedWalkS(this, _rigidbody, _playerInput, _cameraRotation, stateManager);
            groundedRunS = new PGroundedRunS(this, _rigidbody, _playerInput, _cameraRotation, stateManager);

            // --- Falling States ---
            fallingSuperState = new PFallingSuperS(this, _rigidbody, _playerInput, _cameraRotation, stateManager);
            airborneS = new PAirborneS(this, _rigidbody, _playerInput, _cameraRotation, stateManager);
            fallingS = new PFallingS(this, _rigidbody, _playerInput, _cameraRotation, stateManager);
        }
    }

    private void Start()
    {
        stateManager.Initialize(groundedIdleS);
    }

    private void Update()
    {
        stateManager.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateManager.currentState.PhysicsUpdate();
    }
}