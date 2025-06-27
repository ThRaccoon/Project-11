using UnityEngine;

public class PlayerBaseState
{
    // --- Components ---
    protected Player _player;
    protected Rigidbody _rigidbody;
    protected Transform _cameraRotation;
    protected PlayerInput _playerInput;
    protected PlayerStateManager _stateManager;


    // --- variables ---
    protected bool _didPhysicsUpdateRan;
    protected RaycastHit _hitInfo;

    // --- Inputs ---
    protected Vector2 _movementInput;
    protected bool _runInput = false;

    // --- Movement ---
    protected Vector3 _movementVector;


    public PlayerBaseState(Player player, Rigidbody rigidbody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
    {
        // --- Components ---
        _player = player;
        _rigidbody = rigidbody;
        _cameraRotation = cameraRotation;
        _playerInput = playerInput;
        _stateManager = stateManager;
    }


    public virtual void OnEnter()
    {
        _didPhysicsUpdateRan = false;
    }

    public virtual void LogicUpdate()
    {
        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        if (_playerInput != null)
        {
            _movementInput = _playerInput.movementInput;
            _runInput = _playerInput.runInput;
        }

        if (_runInput)
        {
            _player.staminaRegenTimer.Reset();
        }
        else if (_player.staminaRegenTimer.Tick())
        {
            RegenStamina();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public virtual void PhysicsUpdate() { }

    public virtual void OnExit() { }


    protected void RegenStamina()
    {
        if (_player.CurrentStamina < _player.maxStamina)
        {
            _player.CurrentStamina += _player.regenRate * Time.deltaTime;
            _player.CurrentStamina = Mathf.Clamp(_player.CurrentStamina, 0, _player.maxStamina);
        }
    }

    protected void DepleteStamina()
    {
        if (_player.CurrentStamina > 0.0f)
        {
            _player.CurrentStamina -= _player.depleteRate * Time.deltaTime;
            _player.CurrentStamina = Mathf.Clamp(_player.CurrentStamina, 0, _player.maxStamina);
        }
    }

    protected Vector3 ProcessMovementVector(Vector2 movementInput, float movementSpeed)
    {
        Vector3 movementVector = new Vector3(movementInput.x * movementSpeed, 0.0f, movementInput.y * movementSpeed);

        if (_cameraRotation != null)
        {
            movementVector = _cameraRotation.rotation * movementVector;
        }
        return movementVector;
    }

    protected void ApplyMovementForce(Vector3 movementVector)
    {
        if (_rigidbody != null)
        {
            _rigidbody.AddForce(movementVector, ForceMode.Acceleration);
        }
    }

    protected void IsGrounded(float raycastLength)
    {
        _player.IsGrounded = false;

        if (Physics.SphereCast(_player.transform.position, _player.sphereCastRadius, Vector3.down, out RaycastHit hit, _player.sphereCastLength))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            if (angle <= _player.allowedSlopeAngle)
            {
                _hitInfo = hit;
                _player.IsGrounded = true;
            }
        }
    }
}