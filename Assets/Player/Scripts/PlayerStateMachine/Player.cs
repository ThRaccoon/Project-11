using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [Header("Auto Assigned")]
    [SerializeField] private CapsuleCollider _CapsuleCollider = null;
    [SerializeField] private Rigidbody _Rigidbody = null;
    [Header("----------")]
    [SerializeField] private Transform _CameraRotation = null;
    [SerializeField] private PlayerInput _PlayerInput = null;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Pull-Down Force Settings")]
    [field: SerializeField] public float defaultPullDownForce { get; private set; } = 0.0f;
    [field: SerializeField] public float minPullDownForce { get; private set; } = -65.0f;
    [field: SerializeField] public float maxPullDownForce { get; private set; } = -120.0f;

    [Tooltip("The incremental value added to the pull-down force over time.")]
    [field: SerializeField] public float pullDownForceIncrement { get; private set; } = -4.0f;

    [Tooltip("The time interval after which the pull-down force is incremented.")]
    [field: SerializeField] public float forceIncrementTimeInterval { get; private set; } = 0.2f;


    private float _currentPullDownForce = 0.0f;

    public float CurrentPullDownForce
    {
        get => _currentPullDownForce;
        set => _currentPullDownForce = value;
    }


    private float _forceIncrementTimer = 0.0f;

    public float ForceIncrementTimer
    {
        get => _forceIncrementTimer;
        set => _forceIncrementTimer = value;
    }


    private float _accumulatedForceValue = 0.0f;

    public float AccumulatedForceValue
    {
        get => _accumulatedForceValue;
        set => _accumulatedForceValue = value;
    }
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Header("Movement Speeds")]
    [field: SerializeField] public float walkSpeed { get; private set; } = 8.0f;
    [field: SerializeField] public float runSpeed { get; private set; } = 12.0f;

    [Header("----------")]
    [Tooltip("How fast the player stops. Value should be between 0 and 0.9.")]
    [field: SerializeField] public float stoppingForce { get; private set; } = 0.5f;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Header("Ground / Ceiling  Checks")]
    [field: SerializeField] public float standingGroundCheckLength { get; private set; } = 0.125f;


    private bool _isGrounded = false;

    public bool IsGrounded
    {
        get => _isGrounded;
        set => _isGrounded = value;
    }
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Drags ---
    private float _playerDrag = 4.0f;
    private float _playerAngularDrag = 4.0f;


    // --- State Machine / States / States Related --- 
    public PlayerBaseState BaseState { get; private set; } = null;
    public PlayerStateManager StateManager { get; private set; } = null;

    // --- Grounded States ---
    public PGroundedSuperState GroundedSuperState { get; private set; } = null;
    public PLandedS LandedS { get; private set; } = null;
    public PGroundedIdleS GroundedIdleS { get; private set; } = null;
    public PGroundedWalkS GroundedWalkS { get; private set; } = null;

    // --- Falling States ---
    public PFallingSuperState FallingSuperState { get; private set; } = null;
    public PAirborneS AirborneS { get; private set; } = null;
    public PFallingS FallingS { get; private set; } = null;


    private void Awake()
    {
        // --- Components ---
        _CapsuleCollider = GetComponent<CapsuleCollider>();

        _Rigidbody = GetComponent<Rigidbody>();

        // --- Timers ---
        _forceIncrementTimer = forceIncrementTimeInterval;

        // --- Ray Casts ---
        if (_CapsuleCollider != null)
        {
            standingGroundCheckLength += _CapsuleCollider.bounds.extents.y;
        }

        // --- Drags ---
        if (_Rigidbody != null)
        {
            _Rigidbody.linearDamping = _playerDrag;
            _Rigidbody.angularDamping = _playerAngularDrag;
        }


        // --- State Machine / States --- 
        BaseState = new PlayerBaseState(this, _Rigidbody, _PlayerInput, _CameraRotation, StateManager);
        StateManager = new PlayerStateManager();

        // --- Grounded States ---
        GroundedSuperState = new PGroundedSuperState(this, _Rigidbody, _PlayerInput, _CameraRotation, StateManager);
        LandedS = new PLandedS(this, _Rigidbody, _PlayerInput, _CameraRotation, StateManager);
        GroundedIdleS = new PGroundedIdleS(this, _Rigidbody, _PlayerInput, _CameraRotation, StateManager);
        GroundedWalkS = new PGroundedWalkS(this, _Rigidbody, _PlayerInput, _CameraRotation, StateManager);

        // --- Falling States ---
        FallingSuperState = new PFallingSuperState(this, _Rigidbody, _PlayerInput, _CameraRotation, StateManager);
        AirborneS = new PAirborneS(this, _Rigidbody, _PlayerInput, _CameraRotation, StateManager);
        FallingS = new PFallingS(this, _Rigidbody, _PlayerInput, _CameraRotation, StateManager);
    }

    private void Start()
    {
        StateManager.Initialize(GroundedIdleS);
    }

    private void Update()
    {
        StateManager.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateManager.CurrentState.PhysicsUpdate();
    }
}
