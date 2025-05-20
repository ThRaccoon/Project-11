using UnityEngine;
using UnityEngine.AI;

public class InteractDoor : MonoBehaviour, IInteractable
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private MeshCollider _meshCollider;
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
    private MeshCollider[] _MeshColliders;

    private enum DoorState
    {
        Opened, Opening, Closed, Closing, Locked
    }

    private DoorState _currentState = DoorState.Closed;

    private void Awake()
    {
        // --- Components ---
        _meshCollider = GetComponent<MeshCollider>();
        _MeshColliders = GetComponentsInChildren<MeshCollider>();
        _audioSource = GetComponent<AudioSource>();

        if (Util.IsNotNull(_audioSource))
        {
            _audioSource = GetComponentInParent<AudioSource>();
        }

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
                if (Util.IsNotNull(_meshCollider))
                {
                    _meshCollider.enabled = true;
                }

                foreach (MeshCollider meshCollider in _MeshColliders)
                {
                    meshCollider.enabled = true;
                }

                if (_currentState == DoorState.Opening)
                {
                    _currentState = DoorState.Opened;
                }
                else
                {
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
                // _isInitiallyLocked = false;
                //PlaySound(_unlockingSound);

                // if the player don't have the key
                PlaySound(_lockedSound);

            }
            else
            {
                if (_currentState == DoorState.Closed)
                {
                    PlaySound(_openingSound);
                    Rotate(Quaternion.Euler(_initialX, _openAngle, _initialZ));
                    _currentState = DoorState.Opening;
                }
                else if (_currentState == DoorState.Opened)
                {
                    PlaySound(_closingSound);
                    Rotate(Quaternion.Euler(_initialX, _closedAngle, _initialZ));
                    _currentState = DoorState.Closing;
                }
            }
        }
    }

    private void Rotate(Quaternion targetRotation)
    {
        if (Util.IsNotNull(_meshCollider))
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
        if (Util.IsNotNull(_audioSource) && Util.IsNotNull(clip))
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}