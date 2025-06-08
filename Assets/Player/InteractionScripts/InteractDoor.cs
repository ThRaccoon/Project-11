using UnityEngine;
using UnityEngine.AI;

public class InteractDoor : MonoBehaviour, IInteractable
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private BoxCollider _boxCollider;
    [Space(10)]
    [SerializeField] private BoxCollider[] _childBoxColliders;
    [Space(15)]
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
    [SerializeField, Range(0f, 1f)] private float _lockedVolume = 0.5f;
    [Space(10)]
    [SerializeField] private AudioClip _unlockingSound;
    [SerializeField, Range(0f, 1f)] private float _unlockingVolume = 0.5f;
    [Space(10)]
    [SerializeField] private AudioClip _openingSound;
    [SerializeField, Range(0f, 1f)] private float _openingVolume = 0.5f;
    [Space(10)]
    [SerializeField] private AudioClip _openedSound;
    [SerializeField, Range(0f, 1f)] private float _openedVolume = 0.5f;
    [Space(10)]
    [SerializeField] private AudioClip _closingSound;
    [SerializeField, Range(0f, 1f)] private float _closingVolume = 0.5f;
    [Space(10)]
    [SerializeField] private AudioClip _closedSound;
    [SerializeField, Range(0f, 1f)] private float _closedVolume = 0.5f; 
    // ----------------------------------------------------------------------------------------------------------------------------------

    // --- Private Variables ---
    private float SlerpProgress;
    private Quaternion _startRotationPoint;
    private Quaternion _targetRotationPoint;
    private AudioClip _lastClip;

    private enum EDoorState
    {
        Opened, Opening, Closed, Closing, Locked
    }

    private EDoorState _currentState = EDoorState.Closed;

    private void Awake()
    {
        // Assaign Components
        _boxCollider = GetComponent<BoxCollider>();
        _childBoxColliders = GetComponentsInChildren<BoxCollider>();
        _audioSource = GetComponent<AudioSource>();

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
                if (_boxCollider != null)
                {
                    _boxCollider.enabled = true;
                }

                foreach (BoxCollider boxCollider in _childBoxColliders)
                {
                    boxCollider.enabled = true;
                }

                if (_currentState == EDoorState.Opening)
                {
                    _currentState = EDoorState.Opened;
                    PlaySound(_openedSound, _openedVolume);
                }
                else
                {
                    _currentState = EDoorState.Closed;
                    PlaySound(_closedSound, _closedVolume);
                }
            }
        }
    }


    public void Interact()
    {
        if (_isInitiallyLocked)
        {
            // if the player have the key
            // _isInitiallyLocked = false;
            // PlaySound(_unlockingSound);

            // if the player don't have the key
            PlaySound(_lockedSound, _lockedVolume, true);
        }
        else
        {
            if (_currentState == EDoorState.Closed)
            {
                PlaySound(_openingSound, _openingVolume);
                Rotate(Quaternion.Euler(_initialX, _openAngle, _initialZ));
                _currentState = EDoorState.Opening;
            }
            else if (_currentState == EDoorState.Opened)
            {
                PlaySound(_closingSound, _closingVolume);
                Rotate(Quaternion.Euler(_initialX, _closedAngle, _initialZ));
                _currentState = EDoorState.Closing;
            }
        }
    }

    private void Rotate(Quaternion targetRotation)
    {
        if (_boxCollider != null)
        {
            _boxCollider.enabled = false;
        }

        foreach (BoxCollider boxCollider in _childBoxColliders)
        {
            boxCollider.enabled = false;
        }

        SlerpProgress = 0.0f;

        _startRotationPoint = transform.localRotation;

        _targetRotationPoint = targetRotation;
    }

    private void PlaySound(AudioClip clip, float volume, bool shouldRepeat = false)
    {
        if (_audioSource == null && clip == null) return;

        if (clip != _lastClip || shouldRepeat)
        {
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.Play();
        }

        _lastClip = clip;
    }
}