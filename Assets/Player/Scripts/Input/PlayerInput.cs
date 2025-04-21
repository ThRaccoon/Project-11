using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputManager _PlayerInputManager;
    private GameObject _player;

    public Vector2 RotationInput { get; private set; } = Vector2.zero;

    public Vector2 MovementInput { get; private set; } = Vector2.zero;

    public bool RunInput { get; private set; } = false;

    public bool ShootInput { get; private set; } = false;

    public bool UseInput { get; private set; } = false;

    public bool UseInput3 { get; private set; } = false;


    private void Awake()
    {
        _PlayerInputManager = new PlayerInputManager();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        _PlayerInputManager.Enable();

        _PlayerInputManager.OnGround.Rotation.performed += GetRotationInput;
        _PlayerInputManager.OnGround.Rotation.canceled += GetRotationInput;

        _PlayerInputManager.OnGround.Movement.performed += GetMovementInput;
        _PlayerInputManager.OnGround.Movement.canceled += GetMovementInput;

        _PlayerInputManager.OnGround.Run.performed += GetRunInput;
        _PlayerInputManager.OnGround.Run.canceled += GetRunInput;

        _PlayerInputManager.OnGround.Shoot.performed += GetShootInput;
        _PlayerInputManager.OnGround.Shoot.canceled += GetShootInput;

        _PlayerInputManager.OnGround.Use.started += GetUseInput;
        _PlayerInputManager.OnGround.Use.canceled += GetUseInput;
        
        _PlayerInputManager.OnGround._3.started += GetUseInput3;
        _PlayerInputManager.OnGround._3.canceled += GetUseInput3;
    }

    private void OnDisable()
    {
        _PlayerInputManager.OnGround.Rotation.performed -= GetRotationInput;
        _PlayerInputManager.OnGround.Rotation.canceled -= GetRotationInput;

        _PlayerInputManager.OnGround.Movement.performed -= GetMovementInput;
        _PlayerInputManager.OnGround.Movement.canceled -= GetMovementInput;

        _PlayerInputManager.OnGround.Run.performed -= GetRunInput;
        _PlayerInputManager.OnGround.Run.canceled -= GetRunInput;

        _PlayerInputManager.OnGround.Shoot.performed -= GetShootInput;
        _PlayerInputManager.OnGround.Shoot.canceled -= GetShootInput;

        _PlayerInputManager.OnGround.Use.started -= GetUseInput;
        _PlayerInputManager.OnGround.Use.canceled -= GetUseInput;
        
        _PlayerInputManager.OnGround._3.started -= GetUseInput3;
        _PlayerInputManager.OnGround._3.canceled -= GetUseInput3;

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

    private void GetRunInput(InputAction.CallbackContext ctx)
    {
        RunInput = ctx.ReadValueAsButton();
    }

    private void GetShootInput(InputAction.CallbackContext ctx)
    {
        ShootInput = ctx.ReadValueAsButton();
    }

    private void GetUseInput(InputAction.CallbackContext ctx)
    {
        UseInput = ctx.started;
    }  
    
    private void GetUseInput3(InputAction.CallbackContext ctx)
    {

        if(_player !=null)
        {
            var inventoryManager = _player.GetComponent<InventoryManager>();
            if(inventoryManager != null)
            {
                inventoryManager.OpenJournal();
            }
        }
    }
}
