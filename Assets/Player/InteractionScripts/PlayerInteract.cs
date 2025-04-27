using UnityEditor.Playables;
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

    // --- Private Variables ---
    private RaycastHit _hitInfo;
    private GameObject _interactIcon;


    private void Awake()
    {
        _playerCamera = Camera.main;
        _interactIcon = Util.FindSceneObjectByTag("InteractIcon");
    }

    private void FixedUpdate()
    {
        if(Util.IsNotNull(_interactIcon))
        {
            if (Util.IsNotNull(_playerCamera))
            {
                Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out _hitInfo, _interactionRange);
                
                 if (IsInteractable())
                 {
                    _interactIcon.SetActive(true);
                 }
                 else
                 {
                    _interactIcon.SetActive(false);
                 }
            }
        }
    }


    public void PerformInteraction()
    {
        if (Util.IsNotNull(_playerCamera))
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
        if (Util.IsNotNull(_hitInfo.collider))
        {
            IInteractable interactable = _hitInfo.collider.GetComponent<IInteractable>();

            // Do not use NullChecker here to prevent console spamming everytime you press E
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private bool IsInteractable()
    {
        if(_hitInfo.collider != null && _hitInfo.collider.GetComponent<IInteractable>() != null)
        {
            return true;
        }

        return false;
    }

}