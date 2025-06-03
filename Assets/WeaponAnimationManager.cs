using UnityEngine;
using UnityEngine.InputSystem.XR;

public class WeaponAnimationManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private Animator _animator;

    private string _currentAnim;
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


    private EWeaponState _currentState = EWeaponState.Default;

    private void Update()
    {
        if (_inventoryManager.currentWeapon == null || _animator == null || !_inventoryManager.currentWeapon.weaponPrefab.activeInHierarchy)
            return;
       
        if (_timer.Flag)
        {
            if(_currentAnim == "Reload")
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

                    if (_playerInput.movementInput != Vector2.zero)
                    {
                        ChangeState(EWeaponState.Walk);
                    }
                    break;

                case EWeaponState.Walk:

                    PlayAnimation("Walk");

                    if (_playerInput.movementInput == Vector2.zero)
                    {
                        ChangeState(EWeaponState.Idle);
                    }
                    break;

                case EWeaponState.Shoot:

                    PlayAnimation("Shoot");

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
        _animator = animator;
        _timer.Flag = true;

        _currentState = EWeaponState.Pull;
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
}
