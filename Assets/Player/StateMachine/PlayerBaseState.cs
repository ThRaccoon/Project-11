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

    // --- Other ---
    private Vector3[] groundCheckOffsets;

    public PlayerBaseState(Player player, Rigidbody rigidbody, PlayerInput playerInput, Transform cameraRotation, PlayerStateManager stateManager)
    {
        // --- Components ---
        _player = player;
        _rigidbody = rigidbody;
        _cameraRotation = cameraRotation;
        _playerInput = playerInput;
        _stateManager = stateManager;

        // --- Other ---
        groundCheckOffsets = new Vector3[]
        {
        new Vector3(-_player.groundCheckOffset, 0f, 0f),
        new Vector3(_player.groundCheckOffset, 0f, 0f),
        new Vector3(0f, 0f, -_player.groundCheckOffset),
        new Vector3(0f, 0f, _player.groundCheckOffset)
        };
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

        Debug.DrawRay(_player.transform.position, Vector3.down * raycastLength, Color.white, 2.0f);
        if (Physics.Raycast(_player.transform.position, Vector3.down, out RaycastHit hit, raycastLength))
        {
            _hitInfo = hit;
            _player.IsGrounded = true;
            return;
        }

        foreach (Vector3 offset in groundCheckOffsets)
        {
            Debug.DrawRay(_player.transform.position + offset, Vector3.down * raycastLength, Color.red, 2.0f);
            if (Physics.Raycast(_player.transform.position + offset, Vector3.down, out hit, raycastLength))
            {
                _hitInfo = hit;
                _player.IsGrounded = true;
                break;
            }
        }
    }
}