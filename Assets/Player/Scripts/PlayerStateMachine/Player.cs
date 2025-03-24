using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public enum PlayerScale { Standing, Crouching }

public class Player : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [Header("Auto Assigned")]
    [SerializeField] private CapsuleCollider _CapsuleCollider = null;
    [SerializeField] private Rigidbody _Rigidbody = null;
    [Header("----------")]
    [SerializeField] private Transform _CameraRotation = null;
    [SerializeField] public PlayerInput PlayerInput = null;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Pull-Down Force Settings")]
    [SerializeField] private float _defaultPullDownForce = 0.0f;
    [SerializeField] private float _minPullDownForce = -65.0f;
    [SerializeField] private float _maxPullDownForce = -120.0f;

    [Tooltip("The incremental value added to the pull-down force over time.")]
    [SerializeField] private float _pullDownForceIncrement = -4.0f;

    [Tooltip("The time interval after which the pull-down force is incremented.")]
    [SerializeField] private float _forceIncrementTimeInterval = 0.2f;

    [SerializeField] private float _currentPullDownForce = 0.0f; // Debug

    private float _forceIncrementTimer = 0.0f;
    private float _accumulatedForceValue = 0.0f;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Header("Movement Speeds")]
    [field: SerializeField] public float walkSpeed { get; private set; } = 8.0f;
    [field: SerializeField] public float crouchSpeed { get; private set; } = 3.0f;

    [Header("----------")]
    [Tooltip("How fast the player stops. Value should be between 0 and 0.9.")]
    [SerializeField] private float _stoppingForce = 0.5f;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Header("Ground / Ceiling  Checks")]
    [field: SerializeField] public float standingGroundCheckLength { get; private set; } = 0.015f;
    [field: SerializeField] public float crouchingGroundCheckLength { get; private set; } = 0.015f;
    [field: SerializeField] public float crouchingCeilingCheckLength { get; private set; } = 0.5f;

    [Tooltip("The offset of the ceiling check raycast (used to offset the rays diagonally).")]
    [SerializeField] private float _ceilingCheckRaycastOffset = 0.35f;

    private Vector3 _positiveCeilingOffset = Vector3.zero;
    private Vector3 _negativeCeilingOffset = Vector3.zero;
    private Vector3[] _ceilingCheckRaycastOffsets = new Vector3[2];
    private RaycastHit _hitInfo;

    [field: SerializeField] public bool isGrounded { get; private set; } = false;
    [field: SerializeField] public bool canStandUp { get; private set; } = false;
    [field: SerializeField] public float slopeAngle { get; private set; } = 0.0f;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Drags ---
    private float _playerDrag = 4.0f;
    private float _playerAngularDrag = 4.0f;

    // --- Scaling ---
    [SerializeField] private float _crouchedYScale = 0.0f;

    private float _standingScaleY = 0.0f;
    private float _crouchingScaleY = 0.0f;
    private Vector3 _standingScaleVector = Vector3.zero;
    private Vector3 _crouchingScaleVector = Vector3.zero;


    // --- State Machine / States / States Related --- 
    public PlayerStateManager StateManager { get; private set; } = null;

    // --- Grounded States ---
    public PGroundedSuperState GroundedSuperState { get; private set; } = null;
    public PLandedS LandedS { get; private set; } = null;
    public PGroundedStandingIdleS GroundedStandingIdleS { get; private set; } = null;
    public PGroundedStandingWalkS GroundedStandingWalkS { get; private set; } = null;
    public PGroundedCrouchingIdleS GroundedCrouchingIdleS { get; private set; } = null;
    public PGroundedCrouchingWalkS GroundedCrouchingWalkS { get; private set; } = null;

    // --- Falling States ---
    public PFallingSuperState FallingSuperState { get; private set; } = null;
    public PFallingS FallingS { get; private set; } = null;
    public PFallingStandingS FallingStandingS { get; private set; } = null;
    public PFallingCrouchingS FallingCrouchingS { get; private set; } = null;

    // --- States Related ---
    [HideInInspector] public PlayerScale currentPlayerScale;
    [HideInInspector] public PlayerScale previousPlayerScale;

    [HideInInspector] public bool wasPreviousStateFalling = false;


    private void Awake()
    {
        // --- Components ---
        _CapsuleCollider = GetComponent<CapsuleCollider>();

        _Rigidbody = GetComponent<Rigidbody>();

        // --- Scaling ---
        _standingScaleVector = transform.localScale;

        _crouchingScaleVector.Set(transform.localScale.x,
                                  _crouchedYScale,
                                  transform.localScale.z);

        // --- Ray Casts ---
        if (_CapsuleCollider != null)
        {
            standingGroundCheckLength += _CapsuleCollider.bounds.extents.y;
            crouchingGroundCheckLength += _CapsuleCollider.bounds.extents.y / 2;
            crouchingCeilingCheckLength += _CapsuleCollider.bounds.extents.y / 2;
        }

        // --- Timers ---
        _forceIncrementTimer = _forceIncrementTimeInterval;

        // --- Bools ---
        canStandUp = true;

        // --- Drags ---
        if (_Rigidbody != null)
        {
            _Rigidbody.linearDamping = _playerDrag;
            _Rigidbody.angularDamping = _playerAngularDrag;
        }


        // --- State Machine / States --- 
        StateManager = new PlayerStateManager();

        // --- Grounded States ---
        GroundedSuperState = new PGroundedSuperState(this, StateManager, false);
        LandedS = new PLandedS(this, StateManager, false);
        GroundedStandingIdleS = new PGroundedStandingIdleS(this, StateManager, false);
        GroundedStandingWalkS = new PGroundedStandingWalkS(this, StateManager, false);
        GroundedCrouchingIdleS = new PGroundedCrouchingIdleS(this, StateManager, false);
        GroundedCrouchingWalkS = new PGroundedCrouchingWalkS(this, StateManager, false);

        // --- Falling States ---
        FallingSuperState = new PFallingSuperState(this, StateManager, false);
        FallingS = new PFallingS(this, StateManager, false);
        FallingStandingS = new PFallingStandingS(this, StateManager, false);
        FallingCrouchingS = new PFallingCrouchingS(this, StateManager, false);
    }

    private void Start()
    {
        StateManager.Initialize(GroundedStandingIdleS);
    }

    private void Update()
    {
        StateManager.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateManager.CurrentState.PhysicsUpdate();
    }


    // --- Rotation / Movement Input / Movement Speed ---
    public Vector3 ProcessMovementVector(Vector2 movementInput, float movementSpeed)
    {
        Vector3 movementVector = new Vector3(movementInput.x * movementSpeed, 0.0f, movementInput.y * movementSpeed);

        if (_CameraRotation != null)
        {
            movementVector = _CameraRotation.rotation * movementVector;
        }

        return movementVector;
    }


    // --- Apply Forces ---
    public void ApplyMovementForce(Vector3 movementVector)
    {
        if (_Rigidbody != null)
        {
            _Rigidbody.AddForce(movementVector, ForceMode.Acceleration);
        }
    }

    public void ApplyStoppingForce()
    {
        if (_Rigidbody != null)
        {
            _Rigidbody.linearVelocity *= _stoppingForce;
            if (_Rigidbody.linearVelocity.magnitude < 0.01f)
            {
                _Rigidbody.linearVelocity = Vector3.zero;
            }
        }
    }


    // --- Apply Scale ---
    public void ApplyScale(bool isStanding)
    {
        if (isStanding)
        {
            transform.localScale = _standingScaleVector;
            _Rigidbody.position = new Vector3(transform.position.x, transform.position.y + _crouchedYScale, transform.position.z);
        }
        else
        {
            transform.localScale = _crouchingScaleVector;
            _Rigidbody.position = new Vector3(transform.position.x, transform.position.y - _crouchedYScale, transform.position.z);
        }
    }


    // --- Slope Angle / Projection ---
    public void CalculateSlopeAngle()
    {
        slopeAngle = Vector3.Angle(Vector3.up, _hitInfo.normal);
    }

    public Vector3 CalculateSlopeProjection(Vector3 movementVector)
    {
        return Vector3.ProjectOnPlane(movementVector, _hitInfo.normal);
    }


    // --- Pull-Down Force ---
    public float CalculatePullDownForce()
    {
        _forceIncrementTimer -= Time.fixedDeltaTime;
        _currentPullDownForce = _accumulatedForceValue + _minPullDownForce;

        if (_forceIncrementTimer < 0)
        {
            if (_currentPullDownForce > _maxPullDownForce)
            {
                _accumulatedForceValue += _pullDownForceIncrement;
            }

            _forceIncrementTimer = _forceIncrementTimeInterval;
        }
        return _currentPullDownForce;
    }

    public void ResetPullDownForce()
    {
        _accumulatedForceValue = 0;
        _currentPullDownForce = _defaultPullDownForce;
        _forceIncrementTimer = _forceIncrementTimeInterval;
    }


    // --- Ceiling / Ground Checks ---
    public void IsGrounded(float raycastLength)
    {
        isGrounded = false;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, raycastLength))
        {
            _hitInfo = hitInfo;
            isGrounded = true;
        }
    }

    public void CanStandUp()
    {
        canStandUp = true;

        if (PlayerInput != null)
        {
            if (PlayerInput.MovementInput != Vector2.zero)
            {
                Vector3 directionVector = new Vector3(PlayerInput.MovementInput.x, 0.0f, PlayerInput.MovementInput.y);

                if (_CameraRotation != null)
                {
                    _positiveCeilingOffset = _CameraRotation.rotation * (directionVector * _ceilingCheckRaycastOffset);
                    _negativeCeilingOffset = _CameraRotation.rotation * (directionVector * -_ceilingCheckRaycastOffset);
                }

                _ceilingCheckRaycastOffsets[0] = _positiveCeilingOffset;
                _ceilingCheckRaycastOffsets[1] = _negativeCeilingOffset;
            }
        }

        foreach (Vector3 offset in _ceilingCheckRaycastOffsets)
        {
            if (Physics.Raycast(transform.position + offset, Vector3.up, crouchingCeilingCheckLength))
            {
                Debug.DrawRay(transform.position + offset, Vector3.up * crouchingCeilingCheckLength);
                canStandUp = false;
                break;
            }
        }
    }
}
