using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerInteract _playerInteract;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private MenuManager _menuManager;

    private PlayerInputManager _playerInputManager;

    public Vector2 rotationInput { get; private set; }

    public Vector2 movementInput { get; private set; }

    public bool runInput { get; private set; }

    public bool isPaused;

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

        _playerInputManager.OnGround.Q.performed += GetQInput;

        _playerInputManager.OnGround.E.performed += GetEInput;

        _playerInputManager.OnGround.R.performed += GetRInput;

        _playerInputManager.OnGround._1.performed += Get1Input;

        _playerInputManager.OnGround._2.performed += Get2Input;

        _playerInputManager.OnGround._3.performed += Get3Input;

        _playerInputManager.OnGround.Esc.performed += GetEscInput;
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

        _playerInputManager.OnGround.Q.performed -= GetQInput;

        _playerInputManager.OnGround.E.performed -= GetEInput;

        _playerInputManager.OnGround.R.performed -= GetRInput;

        _playerInputManager.OnGround._1.performed -= Get1Input;

        _playerInputManager.OnGround._2.performed -= Get2Input;

        _playerInputManager.OnGround._3.performed -= Get3Input;

        _playerInputManager.OnGround.Esc.performed -= GetEscInput;

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

    private void GetLMBInput(InputAction.CallbackContext ctx)
    {
        if (_inventoryManager != null && !isPaused)
        {
            _inventoryManager.LMB();
        }
    }

    private void GetQInput(InputAction.CallbackContext ctx)
    {
        if (_inventoryManager != null && !isPaused)
        {
            _inventoryManager.EquipNextWeapon();
        }
    }

    private void GetEInput(InputAction.CallbackContext ctx)
    {
        if (_playerInteract != null && !isPaused)
        {
            _playerInteract.PerformInteraction();
        }
    }

    private void GetRInput(InputAction.CallbackContext ctx)
    {
        if (_inventoryManager != null && !isPaused)
        {
            _inventoryManager.StartReload();
        }
    }

    private void Get1Input(InputAction.CallbackContext ctx)
    {
        if (_inventoryManager != null && !isPaused)
        {
            _inventoryManager.EquipWeapon();
        }
    }

    private void Get2Input(InputAction.CallbackContext ctx)
    {
        if (_inventoryManager != null && !isPaused)
        {
            _inventoryManager.EquipFlashlight();
        }
    }

    private void Get3Input(InputAction.CallbackContext ctx)
    {
        if (_inventoryManager != null && !isPaused)
        {
            _inventoryManager.OpenJournal();
        }
    } 
    
    private void GetEscInput(InputAction.CallbackContext ctx)
    {
        if (_inventoryManager != null)
        {
            _menuManager.OnEsc();
        }
    }
}