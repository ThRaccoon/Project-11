using System.Collections;
using System.Collections.Generic;
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
    [Header("----------")]
    [SerializeField] private PlayerInput _PlayerInput = null;
    [SerializeField] private Camera _PlayerCamera = null;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Settings")]
    [SerializeField] private float _interactionRange = 0.0f;
    // ----------------------------------------------------------------------------------------------------------------------------------


    private bool _shouldCastRay = false;
    private bool _didCastRay = false;
    private RaycastHit _hitInfo;


    private void Awake()
    {
        _PlayerCamera = Camera.main;        
    }

    private void Update()
    {
        if (_PlayerInput != null && _PlayerInput.UseInput)
        {   
            _shouldCastRay = true;
            _didCastRay = false;
        }
    }

    private void FixedUpdate()
    {
        if (_shouldCastRay && !_didCastRay)
        {
            if (_PlayerCamera != null)
            {
                Debug.DrawRay(_PlayerCamera.transform.position, _PlayerCamera.transform.forward * _interactionRange);
                Physics.Raycast(_PlayerCamera.transform.position, _PlayerCamera.transform.forward, out _hitInfo, _interactionRange);
            }

            _shouldCastRay = false;
            _didCastRay = true;

            TryToInteract();
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
}
