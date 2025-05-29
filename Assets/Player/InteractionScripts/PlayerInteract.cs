using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class PlayerInteract : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    private Camera _playerCamera;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [Space(30)]
    [Header("Settings")]
    [SerializeField] private float _interactionRange;
    [SerializeField] private GameObject _interactionIcon;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // --- Private Variables ---
    private RaycastHit _hitInfo;

    private void Awake()
    {
        _playerCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (_interactionIcon != null)
        {
            if (_playerCamera != null)
            {
                Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out _hitInfo, _interactionRange);

                if (IsInteractable())
                {
                    _interactionIcon.SetActive(true);
                }
                else
                {
                    _interactionIcon.SetActive(false);
                }
            }
        }
    }


    public void PerformInteraction()
    {
        if (_playerCamera != null)
        {
            Debug.DrawRay(_playerCamera.transform.position, _playerCamera.transform.forward * _interactionRange);

            if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out _hitInfo, _interactionRange))
            {
                TryToInteract();
            }
        }
    }

    private void TryToInteract()
    {
        if (_hitInfo.collider != null)
        {
            IInteractable interactable = _hitInfo.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private bool IsInteractable()
    {
        if (_hitInfo.collider != null && _hitInfo.collider.GetComponent<IInteractable>() != null)
        {
            return true;
        }

        return false;
    }
}