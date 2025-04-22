using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class PlayerInteract : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [Header("Auto Assigned")]
    private Camera _playerCamera = null;
    [Header("----------")]
    [SerializeField] private PlayerInput _playerInput = null;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Space(30)]
    [Header("Settings")]
    [SerializeField] private float _interactionRange = 0.0f;
    // ----------------------------------------------------------------------------------------------------------------------------------


    private bool _shouldCastRay = false;
    private bool _didCastRay = false;
    private RaycastHit _hitInfo;


    private void Awake()
    {
        _playerCamera = Camera.main;
    }

    private void Update()
    {
        if (NullChecker.Check(_playerInput) && _playerInput.UseInput)
        {
            _shouldCastRay = true;
            _didCastRay = false;
        }
    }

    private void FixedUpdate()
    {
        if (_shouldCastRay && !_didCastRay)
        {
            if (NullChecker.Check(_playerCamera))
            {
                Debug.DrawRay(_playerCamera.transform.position, _playerCamera.transform.forward * _interactionRange);

                if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out _hitInfo, _interactionRange))
                {
                    TryToInteract();
                }
            }
            _shouldCastRay = false;
            _didCastRay = true;
        }
    }


    private void TryToInteract()
    {
        if (NullChecker.Check(_hitInfo.collider))
        {
            IInteractable interactable = _hitInfo.collider.GetComponent<IInteractable>();

            if (NullChecker.Check(interactable))
            {
                interactable.Interact();
            }
        }
    }
}