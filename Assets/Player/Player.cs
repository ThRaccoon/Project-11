using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("UI")]
    [SerializeField] private Image staminaImage;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraRotation;
    [SerializeField] private PlayerInput _playerInput;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(30)]
    [field: Header("Walk")]
    [field: SerializeField] public float walkSpeed { get; private set; }

    [field: Space(10)]
    [Tooltip("How fast the player stops. Value should be between 0 and 0.9.")]
    [field: SerializeField] public float stoppingForce { get; private set; }
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Header("Run")]
    [field: SerializeField] public float runSpeed { get; private set; }

    [field: SerializeField] public float maxStamina { get; private set; }
    [field: SerializeField] public float regenRate { get; private set; }
    [field: SerializeField] public float depleteRate { get; private set; }

    [field: SerializeField] public float staminaRegenDelay { get; private set; }
    public GlobalTimer staminaRegenTimer { get; private set; }


    #region Getters / Setters

    [SerializeField] private float _currentStamina;

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
    [field: SerializeField] public float defaultPullDownForce { get; private set; }
    [field: SerializeField] public float minPullDownForce { get; private set; }
    [field: SerializeField] public float maxPullDownForce { get; private set; }

    [Tooltip("The incremental value added to the pull-down force over time.")]
    [field: SerializeField] public float pullDownForceIncrement { get; private set; }

    [Tooltip("The time interval after which the pull-down force is incremented.")]
    [field: SerializeField] public float forceIncrementDelay { get; private set; }
    public GlobalTimer forceIncrementTimer { get; private set; }


    #region Getters / Setters

    [SerializeField] private float _currentPullDownForce;

    public float CurrentPullDownForce
    {
        get => _currentPullDownForce;
        set => _currentPullDownForce = value;
    }

    private float _accumulatedForceValue;

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
    [field: SerializeField] public float standingGroundCheckLength { get; private set; }


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
    public PlayerStateManager stateManager { get; private set; }

    // --- Base State ---
    public PlayerBaseState baseState { get; private set; }

    // --- Grounded States ---
    public PGroundedSuperS groundedSuperState { get; private set; }
    public PLandedS landedS { get; private set; } = null;
    public PGroundedIdleS groundedIdleS { get; private set; }
    public PGroundedWalkS groundedWalkS { get; private set; }
    public PGroundedRunS groundedRunS { get; private set; }

    // --- Falling States ---
    public PFallingSuperS fallingSuperState { get; private set; }
    public PAirborneS airborneS { get; private set; }
    public PFallingS fallingS { get; private set; }
    #endregion


    private void Awake()
    {
        // --- Components ---
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();

        _cameraRotation = GameObject.FindGameObjectWithTag("CameraRotation").transform;
        _playerInput = GameObject.FindGameObjectWithTag("PlayerInput").GetComponent<PlayerInput>();

        // --- Assigned On Start ---
        _currentStamina = maxStamina;

        // --- Timers ---
        forceIncrementTimer = new GlobalTimer(forceIncrementDelay);
        staminaRegenTimer = new GlobalTimer(staminaRegenDelay);

        // --- Ray Casts ---
        if (Util.IsNotNull(_capsuleCollider))
        {
            standingGroundCheckLength += _capsuleCollider.bounds.extents.y;
        }

        // --- Drags ---
        if (Util.IsNotNull(_rigidbody))
        {
            _rigidbody.linearDamping = _playerDrag;
            _rigidbody.angularDamping = _playerAngularDrag;
        }

        if (Util.IsNotNull(_rigidbody) && Util.IsNotNull(_playerInput) && Util.IsNotNull(_cameraRotation))
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
        // staminaImage.fillAmount = CurrentStamina / maxStamina;

        stateManager.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateManager.currentState.PhysicsUpdate();
    }
}