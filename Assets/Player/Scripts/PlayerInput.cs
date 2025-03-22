using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputManager _PlayerInputManager;

    public Vector2 RotationInput { get; private set; } = Vector2.zero;

    public Vector2 MovementInput { get; private set; } = Vector2.zero;

    public bool CrouchInput { get; private set; } = false;

    public bool ShootInput { get; private set; } = false;

    public bool UseInput { get; private set; } = false;


    private void Awake()
    {
        _PlayerInputManager = new PlayerInputManager();
    }

    private void OnEnable()
    {
        _PlayerInputManager.Enable();

        _PlayerInputManager.OnGround.Rotation.performed += GetRotationInput;
        _PlayerInputManager.OnGround.Rotation.canceled += GetRotationInput;

        _PlayerInputManager.OnGround.Movement.performed += GetMovementInput;
        _PlayerInputManager.OnGround.Movement.canceled += GetMovementInput;

        _PlayerInputManager.OnGround.Crouch.performed += GetCrouchInput;
        _PlayerInputManager.OnGround.Crouch.canceled += GetCrouchInput;

        _PlayerInputManager.OnGround.Shoot.performed += GetShootInput;
        _PlayerInputManager.OnGround.Shoot.canceled += GetShootInput;

        _PlayerInputManager.OnGround.Use.started += GetUseInput;
        _PlayerInputManager.OnGround.Use.canceled += GetUseInput;
    }

    private void OnDisable()
    {
        _PlayerInputManager.OnGround.Rotation.performed -= GetRotationInput;
        _PlayerInputManager.OnGround.Rotation.canceled -= GetRotationInput;

        _PlayerInputManager.OnGround.Movement.performed -= GetMovementInput;
        _PlayerInputManager.OnGround.Movement.canceled -= GetMovementInput;

        _PlayerInputManager.OnGround.Crouch.performed -= GetCrouchInput;
        _PlayerInputManager.OnGround.Crouch.canceled -= GetCrouchInput;

        _PlayerInputManager.OnGround.Shoot.performed -= GetShootInput;
        _PlayerInputManager.OnGround.Shoot.canceled -= GetShootInput;

        _PlayerInputManager.OnGround.Use.started -= GetUseInput;
        _PlayerInputManager.OnGround.Use.canceled -= GetUseInput;

        _PlayerInputManager.Disable();
    }


    private void GetRotationInput(InputAction.CallbackContext ctx)
    {
        RotationInput = ctx.ReadValue<Vector2>();
    }

    private void GetMovementInput(InputAction.CallbackContext ctx)
    {
        MovementInput = ctx.ReadValue<Vector2>();
    }

    private void GetCrouchInput(InputAction.CallbackContext ctx)
    {
        CrouchInput = ctx.ReadValueAsButton();
    }

    private void GetShootInput(InputAction.CallbackContext ctx)
    {
        ShootInput = ctx.ReadValueAsButton();
    }

    private void GetUseInput(InputAction.CallbackContext ctx)
    {
        UseInput = ctx.started;
    }
}
