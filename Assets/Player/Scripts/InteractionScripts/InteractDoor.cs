using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class InteractDoor : MonoBehaviour, IInteractable
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [Header("Auto Assigned")]
    [SerializeField] private BoxCollider _BoxCollider = null;
    [Header("----------")]
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Settings")]
    [SerializeField] private bool _isInitiallyOpened = false;
    [SerializeField] private bool _isOpenDirectionReversed = false;
    [SerializeField] private float _openAngle = 0.0f;
    [SerializeField] private float _closeAngle = 0.0f;
    [SerializeField] private float _rotationSpeed = 0.0f;
    [SerializeField] private float _initialX = 0.0f;
    [SerializeField] private float _initialZ = 0.0f;
    // ----------------------------------------------------------------------------------------------------------------------------------


    private float _rotationLerpProgress = 0.0f;
    private Quaternion _startRotationPoint = Quaternion.identity;
    private Quaternion _targetRotationPoint = Quaternion.identity;
    BoxCollider[] boxColliders;

    private enum DoorState
    {
        Opened, Opening, Closed, Closing
    }

    private DoorState _currentState;


    private void Awake()
    {
        // --- Components ---
        _BoxCollider = GetComponent<BoxCollider>();

        boxColliders = GetComponentsInChildren<BoxCollider>();

        _currentState = _isInitiallyOpened ? DoorState.Opened : DoorState.Closed;

        if (_isOpenDirectionReversed)
        {
            _openAngle *= -1.0f;
        }
    }

    private void Update()
    {
        if (_currentState == DoorState.Opening || _currentState == DoorState.Closing)
        {
            _rotationLerpProgress += _rotationSpeed * Time.deltaTime;

            transform.localRotation = Quaternion.Slerp(_startRotationPoint, _targetRotationPoint, _rotationLerpProgress);

            if (_rotationLerpProgress >= 1.0f)
            {
                if (_BoxCollider != null)
                {
                    _BoxCollider.enabled = true;
                }

                foreach (BoxCollider meshCollider in boxColliders)
                {
                    meshCollider.enabled = true;
                }

                // Play Sound
                _currentState = _currentState == DoorState.Opening ? DoorState.Opened : DoorState.Closed;
            }
        }
    }


    public void Interact()
    {
        if (this.enabled)
        {
            if (_currentState == DoorState.Closed)
            {
                Rotate(Quaternion.Euler(_initialX, _openAngle, _initialZ));
                _currentState = DoorState.Opening;
            }
            else if (_currentState == DoorState.Opened)
            {
                Rotate(Quaternion.Euler(_initialX, _closeAngle, _initialZ));
                _currentState = DoorState.Closing;
            }
        }
    }

    private void Rotate(Quaternion targetRotation)
    {
        if (_BoxCollider != null)
        {
            _BoxCollider.enabled = false;
        }

        foreach (BoxCollider meshCollider in boxColliders)
        {
            meshCollider.enabled = false;
        }

        _rotationLerpProgress = 0.0f;

        _startRotationPoint = transform.localRotation;

        _targetRotationPoint = targetRotation;
    }
}

