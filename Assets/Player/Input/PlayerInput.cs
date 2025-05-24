using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerInteract _playerInteract;
    [SerializeField] private InventoryManager _inventoryManager;

    private PlayerInputManager _playerInputManager;

    public Vector2 rotationInput { get; private set; }

    public Vector2 movementInput { get; private set; }

    public bool runInput { get; private set; }

    public bool shootInput { get; private set; }


    private void Awake()
    {
        _playerInputManager = new PlayerInputManager();
    }

    private void OnEnable()
    {
        _playerInputManager.Enable();

        _playerInputManager.OnGround.Mouse.performed += GetRotationInput;
        _playerInputManager.OnGround.Mouse.canceled += GetRotationInput;

        _playerInputManager.OnGround.WASD.performed += GetMovementInput;
        _playerInputManager.OnGround.WASD.canceled += GetMovementInput;

        _playerInputManager.OnGround.LShift.performed += GetRunInput;
        _playerInputManager.OnGround.LShift.canceled += GetRunInput;

        _playerInputManager.OnGround.LMB.performed += GetLMBInput;

        _playerInputManager.OnGround.E.performed += GetUseInput;

        _playerInputManager.OnGround._2.performed += GetUseInput2;

        _playerInputManager.OnGround._3.performed += GetUseInput3;

    }

    private void OnDisable()
    {
        _playerInputManager.OnGround.Mouse.performed -= GetRotationInput;
        _playerInputManager.OnGround.Mouse.canceled -= GetRotationInput;

        _playerInputManager.OnGround.WASD.performed -= GetMovementInput;
        _playerInputManager.OnGround.WASD.canceled -= GetMovementInput;

        _playerInputManager.OnGround.LShift.performed -= GetRunInput;
        _playerInputManager.OnGround.LShift.canceled -= GetRunInput;

        _playerInputManager.OnGround.LMB.performed -= GetLMBInput;

        _playerInputManager.OnGround.E.performed -= GetUseInput;

        _playerInputManager.OnGround._2.performed -= GetUseInput2;

        _playerInputManager.OnGround._3.performed -= GetUseInput3;

        _playerInputManager.Disable();
    }


    private void GetRotationInput(InputAction.CallbackContext ctx)
    {
        rotationInput = ctx.ReadValue<Vector2>();
    }

    private void GetMovementInput(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    private void GetRunInput(InputAction.CallbackContext ctx)
    {
        runInput = ctx.ReadValueAsButton();
    }

    private void GetUseInput(InputAction.CallbackContext ctx)
    {
        if (_playerInteract != null)
        {
            _playerInteract.PerformInteraction();
        }
    }

    private void GetLMBInput(InputAction.CallbackContext ctx)
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.LMB();
        }
    }

    private void GetUseInput2(InputAction.CallbackContext ctx)
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.EquipFlashlight();
        }
    }

    private void GetUseInput3(InputAction.CallbackContext ctx)
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.OpenJournal();
        }
    }
}