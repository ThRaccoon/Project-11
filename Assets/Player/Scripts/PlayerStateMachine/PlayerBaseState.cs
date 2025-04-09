using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBaseState
{
    // --- Components ---
    protected Player _Player = null;
    protected Rigidbody _Rigidbody = null;
    protected PlayerInput _PlayerInput = null;
    protected PlayerStateManager _StateManager = null;
    protected Transform _CameraRotation = null;

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
        _Player = player;
        _Rigidbody = rigidbody;
        _PlayerInput = playerInput;
        _StateManager = stateManager;
        _CameraRotation = cameraRotation;

        _didPhysicsUpdateRan = false;
    }


    public virtual void OnEnter()
    {
        _didPhysicsUpdateRan = false;
    }

    public virtual void LogicUpdate()
    {
        _movementInput = _PlayerInput.MovementInput;
        _runInput = _PlayerInput.RunInput;

        if (_runInput)
        {
            _Player.staminaRegenTimer.Reset();
        }
        else if (_Player.staminaRegenTimer.CountDownTimer())
        {
            RegenStamina();
        }
    }

    public virtual void PhysicsUpdate() { }

    public virtual void OnExit() { }


    protected Vector3 ProcessMovementVector(Vector2 movementInput, float movementSpeed)
    {
        Vector3 movementVector = new Vector3(movementInput.x * movementSpeed, 0.0f, movementInput.y * movementSpeed);

        if (_CameraRotation != null)
        {
            movementVector = _CameraRotation.rotation * movementVector;
        }

        return movementVector;
    }

    protected void ApplyMovementForce(Vector3 movementVector)
    {
        if (_Rigidbody != null)
        {
            _Rigidbody.AddForce(movementVector, ForceMode.Acceleration);
        }
    }

    protected void RegenStamina()
    {
        if (_Player.CurrentStamina < _Player.maxStamina)
        {
            _Player.CurrentStamina += _Player.regenRate * Time.deltaTime;
            _Player.CurrentStamina = Mathf.Clamp(_Player.CurrentStamina, 0, _Player.maxStamina);
        }
    }

    protected void DepleteStamina()
    {
        if (_Player.CurrentStamina > 0.0f)
        {
            _Player.CurrentStamina -= _Player.depleteRate * Time.deltaTime;
            _Player.CurrentStamina = Mathf.Clamp(_Player.CurrentStamina, 0, _Player.maxStamina);
        }
    }

    protected void IsGrounded(float raycastLength)
    {
        _Player.IsGrounded = false;

        Debug.DrawRay(_Player.transform.position, Vector3.down * raycastLength, Color.red, 2.0f);
        if (Physics.Raycast(_Player.transform.position, Vector3.down, out RaycastHit hitInfo, raycastLength))
        {
            _hitInfo = hitInfo;
            _Player.IsGrounded = true;
        }
    }
}

