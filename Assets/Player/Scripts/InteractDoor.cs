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
    [SerializeField] private AudioSource _AudioSource = null;
    [Header("----------")]
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Space(30)]
    [Header("Settings")]
    [SerializeField] private bool _isInitiallyLocked = false;
    [SerializeField] private bool _isInitiallyOpened = false;

    [Space(10)]
    [Header("Rotation Settings")]
    [SerializeField] private float _openAngle = 0.0f;
    [SerializeField] private float _closedAngle = 0.0f;
    [SerializeField] private float _rotationSpeed = 0.0f;

    [Space(10)]
    [Header("Advanced Settings")]
    [SerializeField] private bool _isOpenDirectionReversed = false;
    [SerializeField] private float _initialX = 0.0f;
    [SerializeField] private float _initialZ = 0.0f;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Space(30)]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _lockedSound = null;
    [SerializeField] private AudioClip _unlockingSound = null;
    [SerializeField] private AudioClip _openingSound = null;
    [SerializeField] private AudioClip _closingSound = null;
    // ----------------------------------------------------------------------------------------------------------------------------------


    private float SlerpProgress = 0.0f;
    private Quaternion _startRotationPoint = Quaternion.identity;
    private Quaternion _targetRotationPoint = Quaternion.identity;
    private BoxCollider[] _BoxColliders;


    private enum DoorState
    {
        Opened, Opening, Closed, Closing, Locked
    }

    private DoorState _currentState = DoorState.Closed;


    private void Awake()
    {
        // --- Components ---
        _BoxCollider = GetComponent<BoxCollider>();
        _BoxColliders = GetComponentsInChildren<BoxCollider>();
        _AudioSource = GetComponent<AudioSource>();

        // --- Bools ---
        _currentState = _isInitiallyOpened ? DoorState.Opened : DoorState.Closed;

        // --- Logic ---
        if (_isOpenDirectionReversed)
        {
            _openAngle *= -1.0f;
        }
    }

    private void Update()
    {
        if (_currentState == DoorState.Opening || _currentState == DoorState.Closing)
        {
            SlerpProgress += _rotationSpeed * Time.deltaTime;

            transform.localRotation = Quaternion.Slerp(_startRotationPoint, _targetRotationPoint, SlerpProgress);

            if (SlerpProgress >= 1.0f)
            {
                if (_BoxCollider != null)
                {
                    _BoxCollider.enabled = true;
                }

                foreach (BoxCollider meshCollider in _BoxColliders)
                {
                    meshCollider.enabled = true;
                }

                if (_currentState == DoorState.Opening)
                {
                    PlaySound(_openingSound);
                    _currentState = DoorState.Opened;
                }
                else
                {
                    PlaySound(_closingSound);
                    _currentState = DoorState.Closed;
                }
            }
        }
    }


    public void Interact()
    {
        if (this.enabled)
        {
            if (_isInitiallyLocked)
            {
                // if the player have the key
                _isInitiallyLocked = false;
                PlaySound(_unlockingSound);

                // if the player don't have the key
                PlaySound(_lockedSound);

            }
            else
            {
                if (_currentState == DoorState.Closed)
                {
                    Rotate(Quaternion.Euler(_initialX, _openAngle, _initialZ));
                    _currentState = DoorState.Opening;
                }
                else if (_currentState == DoorState.Opened)
                {
                    Rotate(Quaternion.Euler(_initialX, _closedAngle, _initialZ));
                    _currentState = DoorState.Closing;
                }
            }
        }
    }


    private void Rotate(Quaternion targetRotation)
    {
        if (_BoxCollider != null)
        {
            _BoxCollider.enabled = false;
        }

        foreach (BoxCollider meshCollider in _BoxColliders)
        {
            meshCollider.enabled = false;
        }

        SlerpProgress = 0.0f;

        _startRotationPoint = transform.localRotation;

        _targetRotationPoint = targetRotation;
    }


    private void PlaySound(AudioClip clip)
    {
        if (_AudioSource != null && clip != null)
        {
            _AudioSource.PlayOneShot(clip);
        }
    }
}