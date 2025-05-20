using UnityEngine;

public class InteractDrawer : MonoBehaviour, IInteractable
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [Header("Auto Assigned")]
    [SerializeField] private AudioSource _audioSource;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [Space(30)]
    [Header("Settings")]
    [SerializeField] private bool _isInitiallyLocked;
    [SerializeField] private bool _isInitiallyOpened;

    [Space(10)]
    [Header("Slide Settings")]
    [SerializeField] private float _openZPosition;
    [SerializeField] private float _closedZPosition;
    [SerializeField] private float _slideSpeed;
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
    private float _lerpProgress;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    private enum DrawerState
    {
        Opened, Opening, Closed, Closing, Locked
    }

    private DrawerState _currentState = DrawerState.Closed;

    private void Awake()
    {
        // --- Components ---
        _audioSource = GetComponent<AudioSource>();
        
        if (Util.IsNotNull(_audioSource))
        {
            _audioSource = GetComponentInParent<AudioSource>();
        }

        // --- Bools ---
        _currentState = _isInitiallyOpened ? DrawerState.Opened : DrawerState.Closed;
    }

    private void Update()
    {
        if (_currentState == DrawerState.Opening || _currentState == DrawerState.Closing)
        {
            _lerpProgress += _slideSpeed * Time.deltaTime;

            transform.localPosition = Vector3.Lerp(_startPosition, _targetPosition, _lerpProgress);

            if (_lerpProgress >= 1.0f)
            {
                if (_currentState == DrawerState.Opening)
                {
                    PlaySound(_openingSound);
                    _currentState = DrawerState.Opened;
                }
                else
                {
                    PlaySound(_closingSound);
                    _currentState = DrawerState.Closed;
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
                if (_currentState == DrawerState.Closed)
                {
                    Slide(new Vector3(transform.localPosition.x, transform.localPosition.y, _openZPosition));
                    _currentState = DrawerState.Opening;
                }
                else if (_currentState == DrawerState.Opened)
                {
                    Slide(new Vector3(transform.localPosition.x, transform.localPosition.y, _closedZPosition));
                    _currentState = DrawerState.Closing;
                }
            }
        }
    }

    private void Slide(Vector3 targetPosition)
    {
        _lerpProgress = 0.0f;

        _startPosition = transform.localPosition;

        _targetPosition = targetPosition;
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