using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public class WeaponAnimationManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioSource _walkAudioSource;
    [SerializeField] private AudioClip _walkSound;
    [SerializeField] private float _walkSoundInterval;
    [SerializeField, Range(0f, 1f)] private float _walkVolume = 0.5f;
    [SerializeField] private AudioClip _runSound;
    [SerializeField] private float _runSoundInterval;
    [SerializeField, Range(0f, 1f)] private float _runVolume = 0.5f;
    public enum EWeaponState
    {
        Default,
        Idle,
        Pull,
        Walk,
        Shoot,
        Reload

    }

    private GlobalTimer _timer = new GlobalTimer(0f, true);
    private GlobalTimer _walkTimer = new GlobalTimer(0f, true);
    private string _currentAnim;
    private string _currentAudioClip;
    private WeaponData _weapon;



    private EWeaponState _currentState = EWeaponState.Default;

    private void Update()
    {
        PlayWalkSounds();

        if (_weapon == null || _animator == null || !_weapon.weaponPrefab.activeInHierarchy)
            return;
       
        if (_timer.Flag)
        {

            if (_currentAnim == "Reload")
            {
                _inventoryManager.EndReload();
            }

            _inventoryManager.currentWeapon.canShoot = true;
            switch (_currentState)
            {
                case EWeaponState.Pull:

                    PlayAnimation("Pull");
                    ChangeState(EWeaponState.Idle);

                    break;

                case EWeaponState.Idle:
                    PlayAnimation("Idle");
                    _currentAudioClip = "";

                    if (_playerInput.movementInput != Vector2.zero)
                    {
                        ChangeState(EWeaponState.Walk);
                    }
                    break;

                case EWeaponState.Walk:

                    PlayAnimation("Walk");
                    _currentAudioClip = "";

                    if (_playerInput.movementInput == Vector2.zero)
                    {
                        ChangeState(EWeaponState.Idle);
                    }
                    break;

                case EWeaponState.Shoot:

                    PlayAnimation("Shoot");
                    PlaySound(_weapon.shootSound, _weapon.shootVolume);


                    if (_playerInput.movementInput == Vector2.zero)
                    {
                        ChangeState(EWeaponState.Idle);
                    }

                    if (_playerInput.movementInput != Vector2.zero)
                    {
                        ChangeState(EWeaponState.Walk);
                    }

                    break;

                case EWeaponState.Reload:

                    PlaySound(_weapon.reloadSound, _weapon.reloadVolume);
                    PlayAnimation("Reload");
                    ChangeState(EWeaponState.Idle);
                    
                    break;
            }
        }
        else
        {
            _timer.Tick();
        }

    }


    public void OnWeaponEnabled(Animator animator)
    {
        _currentAnim = "Default";
        _currentAudioClip = "";
        _animator = animator;
        _timer.Flag = true;

        _currentState = EWeaponState.Pull;
        _weapon = _inventoryManager.currentWeapon;
    }

    public void ChangeState(EWeaponState newState)
    {
        _currentState = newState;
    }


    private void PlayAnimation(string animName)
    {
        if (_currentAnim == animName) return;

        if (_animator != null && _animator.gameObject.activeInHierarchy)
        {
            _animator.Play(animName);
            _currentAnim = animName;

            if (animName == "Pull")
            {
                _inventoryManager.currentWeapon.canShoot = false;
                _timer.Duration = _inventoryManager.currentWeapon.pullDuration;
                _timer.Reset();
            }
            else if (animName == "Shoot")
            {
                _inventoryManager.currentWeapon.canShoot = false;
                _timer.Duration = _inventoryManager.currentWeapon.shootDuration;
                _timer.Reset();
            }
            else if (animName == "Reload")
            {
                _inventoryManager.currentWeapon.canShoot = false;
                _timer.Duration = _inventoryManager.currentWeapon.reloadDuration;
                _timer.Reset();
            }
        }
    }

    private void PlaySound(AudioClip audioClip, float volume)
    {
        if (_audioSource != null && audioClip != null && audioClip.name != _currentAudioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.volume = volume;
            _audioSource.Play();
            _currentAudioClip = audioClip.name;
        }
    }

    private void PlayWalkSounds()
    {
        _walkTimer.Tick();

        if (_playerInput.movementInput != Vector2.zero && _walkTimer.Flag && _walkAudioSource != null)
        {
            if( _playerInput.runInput == true && _player.CurrentStamina > 0 && _runSound != null) 
            {
                _walkAudioSource.clip = _runSound;
                _walkAudioSource.volume = _runVolume;
                _walkAudioSource.Play();
                _walkTimer.Duration= _runSoundInterval;
                _walkTimer.Reset();
            }
            else if (_walkSound != null)
            {
                _walkAudioSource.clip = _walkSound;
                _walkAudioSource.volume = _walkVolume;
                _walkAudioSource.Play();
                _walkTimer.Duration = _walkSoundInterval;
                _walkTimer.Reset();
            }
         
        }

    }
}
