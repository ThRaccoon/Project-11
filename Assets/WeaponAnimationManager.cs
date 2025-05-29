using UnityEngine;

public class WeaponAnimationManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InventoryManager _inventoryManager;
    private Animator _animator;

    private string _currentAnim;


    public enum EWeaponState
    {
        Default,
        Pull,
        Idle, Walk, Run, Shoot,
        Reload,
        IdleEmpty, WalkEmpty, RunEmpty

    }

    private EWeaponState _currentState = EWeaponState.Default;

    private void Update()
    {
        switch (_currentState)
        {
            case EWeaponState.Pull:
                _inventoryManager.SetCanShoot(false, _inventoryManager.currentWeapon.weaponType);
                PlayAnimation("Pull");

                if (IsAnimationFinished("Pull"))
                {
                    _inventoryManager.SetCanShoot(true, _inventoryManager.currentWeapon.weaponType);

                    if (_inventoryManager.currentWeapon.ammoInMagazine > 0)
                    {
                        ChangeState(EWeaponState.Idle);
                    }
                    else
                    {
                        ChangeState(EWeaponState.IdleEmpty);
                    }
                }

                break;

            case EWeaponState.Idle:
                PlayAnimation("Idle");

                if (_playerInput.movementInput != Vector2.zero && _playerInput.runInput != true)
                {
                    ChangeState(EWeaponState.Walk);
                }

                if (_playerInput.movementInput != Vector2.zero && _playerInput.runInput == true && _player.CurrentStamina > 0)
                {
                    ChangeState(EWeaponState.Run);
                }

                break;

            case EWeaponState.Walk:
                PlayAnimation("Walk");

                if (_playerInput.movementInput == Vector2.zero)
                {
                    ChangeState(EWeaponState.Idle);
                }

                if (_playerInput.runInput == true && _player.CurrentStamina > 0)
                {
                    ChangeState(EWeaponState.Run);
                }

                break;

            case EWeaponState.Run:
                PlayAnimation("Run");

                if (_playerInput.movementInput == Vector2.zero)
                {
                    ChangeState(EWeaponState.Idle);
                }

                if (_playerInput.runInput != true || _player.CurrentStamina <= 0)
                {
                    ChangeState(EWeaponState.Walk);
                }

                break;

            case EWeaponState.Shoot:
                _inventoryManager.SetCanShoot(false, _inventoryManager.currentWeapon.weaponType);
                PlayAnimation("Shoot");

                if (IsAnimationFinished("Shoot"))
                {
                    _inventoryManager.SetCanShoot(true, _inventoryManager.currentWeapon.weaponType);

                    if (_playerInput.movementInput == Vector2.zero)
                    {
                        ChangeState(EWeaponState.Idle);
                    }

                    if (_playerInput.movementInput != Vector2.zero && _playerInput.runInput != true)
                    {
                        ChangeState(EWeaponState.Walk);
                    }

                    if (_playerInput.movementInput != Vector2.zero && _playerInput.runInput == true && _player.CurrentStamina > 0)
                    {
                        ChangeState(EWeaponState.Run);
                    }
                }

                break;

            case EWeaponState.Reload:
                _inventoryManager.SetCanShoot(false, _inventoryManager.currentWeapon.weaponType);
                PlayAnimation("Reload");

                if (IsAnimationFinished("Reload"))
                {
                    _inventoryManager.EndReload();

                    _inventoryManager.SetCanShoot(true, _inventoryManager.currentWeapon.weaponType);

                    ChangeState(EWeaponState.Idle);
                }

                break;

            case EWeaponState.IdleEmpty:
                PlayAnimation("IdleEmpty");

                if (_playerInput.movementInput != Vector2.zero && _playerInput.runInput != true)
                {
                    ChangeState(EWeaponState.WalkEmpty);
                }

                if (_playerInput.movementInput != Vector2.zero && _playerInput.runInput == true && _player.CurrentStamina > 0)
                {
                    ChangeState(EWeaponState.RunEmpty);
                }

                break;

            case EWeaponState.WalkEmpty:
                PlayAnimation("WalkEmpty");

                if (_playerInput.movementInput == Vector2.zero)
                {
                    ChangeState(EWeaponState.IdleEmpty);
                }

                if (_playerInput.runInput == true && _player.CurrentStamina > 0)
                {
                    ChangeState(EWeaponState.RunEmpty);
                }

                break;

            case EWeaponState.RunEmpty:
                PlayAnimation("RunEmpty");

                if (_playerInput.movementInput == Vector2.zero)
                {
                    ChangeState(EWeaponState.IdleEmpty);
                }

                if (_playerInput.runInput != true || _player.CurrentStamina <= 0)
                {
                    ChangeState(EWeaponState.WalkEmpty);
                }

                break;
        }
    }


    public void OnWeaponEnabled(Animator animator)
    {
        _animator = animator;

        _currentState = EWeaponState.Pull;
    }

    public void OnWeaponDisable(Animator animator)
    {
        _animator = null;

        _currentState = EWeaponState.Default;
    }

    public void ChangeState(EWeaponState newState)
    {
        _currentState = newState;
    }

    private bool IsAnimationFinished(string stateName)
    {
        if (_animator == null) return false;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (_animator.IsInTransition(0)) return false;

        if (!stateInfo.IsName(stateName)) return false;

        return stateInfo.normalizedTime >= 1f;
    }

    private void PlayAnimation(string animName, float fadeValue = 0.4f)
    {
        if (_currentAnim == animName) return;

        if (_animator != null && _animator.gameObject.activeInHierarchy)
        {
            _animator.CrossFade(animName, fadeValue);
            _currentAnim = animName;
        }
    }
}
