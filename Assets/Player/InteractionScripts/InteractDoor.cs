using UnityEngine;
using UnityEngine.AI;

public class InteractDoor : MonoBehaviour, IInteractable
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private MeshCollider _meshCollider;
    [SerializeField] private MeshCollider[] _MeshColliders;
    [SerializeField] private AudioSource _audioSource;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Space(30)]
    [Header("Settings")]
    [SerializeField] private bool _isInitiallyLocked;
    [SerializeField] private bool _isInitiallyOpened;

    [Space(10)]
    [Header("Rotation Settings")]
    [SerializeField] private float _openAngle;
    [SerializeField] private float _closedAngle;
    [SerializeField] private float _rotationSpeed;

    [Space(10)]
    [Header("Advanced Settings")]
    [SerializeField] private bool _isOpenDirectionReversed;
    [SerializeField] private float _initialX;
    [SerializeField] private float _initialZ;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Space(30)]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _lockedSound;
    [SerializeField] private AudioClip _unlockingSound;
    [SerializeField] private AudioClip _openingSound;
    [SerializeField] private AudioClip _closingSound;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Private Variables ---
    private float SlerpProgress;
    private Quaternion _startRotationPoint;
    private Quaternion _targetRotationPoint;

    private enum EDoorState
    {
        Opened, Opening, Closed, Closing, Locked
    }

    private EDoorState _currentState = EDoorState.Closed;

    private void Awake()
    {
        // --- Bools ---
        _currentState = _isInitiallyOpened ? EDoorState.Opened : EDoorState.Closed;

        // --- Logic ---
        if (_isOpenDirectionReversed)
        {
            _openAngle *= -1.0f;
        }
    }

    private void Update()
    {
        if (_currentState == EDoorState.Opening || _currentState == EDoorState.Closing)
        {
            SlerpProgress += _rotationSpeed * Time.deltaTime;

            transform.localRotation = Quaternion.Slerp(_startRotationPoint, _targetRotationPoint, SlerpProgress);

            if (SlerpProgress >= 1.0f)
            {
                if (_meshCollider != null)
                {
                    _meshCollider.enabled = true;
                }

                foreach (MeshCollider meshCollider in _MeshColliders)
                {
                    meshCollider.enabled = true;
                }

                if (_currentState == EDoorState.Opening)
                {
                    _currentState = EDoorState.Opened;
                }
                else
                {
                    _currentState = EDoorState.Closed;
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
                // _isInitiallyLocked = false;
                //PlaySound(_unlockingSound);

                // if the player don't have the key
                PlaySound(_lockedSound);

            }
            else
            {
                if (_currentState == EDoorState.Closed)
                {
                    PlaySound(_openingSound);
                    Rotate(Quaternion.Euler(_initialX, _openAngle, _initialZ));
                    _currentState = EDoorState.Opening;
                }
                else if (_currentState == EDoorState.Opened)
                {
                    PlaySound(_closingSound);
                    Rotate(Quaternion.Euler(_initialX, _closedAngle, _initialZ));
                    _currentState = EDoorState.Closing;
                }
            }
        }
    }

    private void Rotate(Quaternion targetRotation)
    {
        if (_meshCollider != null)
        {
            _meshCollider.enabled = false;
        }

        foreach (MeshCollider meshCollider in _MeshColliders)
        {
            meshCollider.enabled = false;
        }

        SlerpProgress = 0.0f;

        _startRotationPoint = transform.localRotation;

        _targetRotationPoint = targetRotation;
    }

    private void PlaySound(AudioClip clip)
    {
        if (_audioSource != null && clip != null)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}