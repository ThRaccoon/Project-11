using UnityEngine;

public class PlayerBaseState
{
    // --- Components ---
    protected Player _player = null;
    protected Rigidbody _rigidbody = null;
    protected Transform _cameraRotation = null;
    protected PlayerInput _playerInput = null;
    protected PlayerStateManager _stateManager = null;


    // --- variables ---
    protected bool _didPhysicsUpdateRan = false;
    protected RaycastHit _hitInfo;

    // --- Inputs ---
    protected Vector2 _movementInput = Vector2.zero;
    protected bool _runInput = false;

    // --- Movement ---
    protected Vector3 _movementVector = Vector3.zero;


    public PlayerBaseState(Player player, Rigidbody rigidbody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
    {
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
        if (NullChecker.Check(_playerInput))
        {
            _movementInput = _playerInput.movementInput;
            _runInput = _playerInput.runInput;
        }

        if (_runInput)
        {
            _player.staminaRegenTimer.Reset();
        }
        else if (_player.staminaRegenTimer.CountDownTimer())
        {
            RegenStamina();
        }
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public virtual void PhysicsUpdate() { }

    public virtual void OnExit() { }


    protected Vector3 ProcessMovementVector(Vector2 movementInput, float movementSpeed)
    {
        Vector3 movementVector = new Vector3(movementInput.x * movementSpeed, 0.0f, movementInput.y * movementSpeed);

        if (NullChecker.Check(_cameraRotation))
        {
            movementVector = _cameraRotation.rotation * movementVector;
        }
        return movementVector;
    }

    protected void ApplyMovementForce(Vector3 movementVector)
    {
        if (NullChecker.Check(_rigidbody))
        {
            _rigidbody.AddForce(movementVector, ForceMode.Acceleration);
        }
    }

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

    protected void IsGrounded(float raycastLength)
    {
        _player.IsGrounded = false;

        Debug.DrawRay(_player.transform.position, Vector3.down * raycastLength, Color.red, 2.0f);
        if (Physics.Raycast(_player.transform.position, Vector3.down, out RaycastHit hit, raycastLength))
        {
            _hitInfo = hit;
            _player.IsGrounded = true;
        }
    }
}