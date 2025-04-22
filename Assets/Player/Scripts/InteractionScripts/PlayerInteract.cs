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
    // ----------------------------------------------------------------------------------------------------------------------------------

    private RaycastHit _hitInfo;


    private void Awake()
    {
        _playerCamera = Camera.main;
    }


    public void PerformInteraction()
    {
        if (NullChecker.Check(_playerCamera))
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
        if (NullChecker.Check(_hitInfo.collider))
        {
            IInteractable interactable = _hitInfo.collider.GetComponent<IInteractable>();

            // Do not use NullChecker here to prevent console spamming everytime you press E
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}