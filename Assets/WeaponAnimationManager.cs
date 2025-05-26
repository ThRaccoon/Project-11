using UnityEngine;

public class WeaponAnimationManager : MonoBehaviour
{
    [SerializeField] EWeaponType _weaponType;
    [SerializeField] InventoryManager _inventoryManager;
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] Player _player;
    public Animator _animator;
    private bool _playIdle = false;
    private string _lastIdle;
    private EIdleState _idleState;
    public enum EIdleState
    {
        idle = 0,
        idleEmpty = 1
    }

    private void Update()
    {
        if (_playIdle)
        {
            switch (_idleState)
            {
                case EIdleState.idle:
                    {
                        if (_playerInput != null)
                        {
                            if (_playerInput.movementInput != Vector2.zero && _playerInput.runInput && _player != null && _player.CurrentStamina > 0)
                            {
                                PlayIdle("IdleRun");
                            }
                            else if (_playerInput.movementInput != Vector2.zero)
                            {
                                PlayIdle("IdleWalk");
                            }
                            else
                            {
                                PlayIdle("Idle");
                            }
                        }

                    }
                    break;
                case EIdleState.idleEmpty:
                    {
                        if (_playerInput != null)
                        {
                            if (_playerInput.movementInput != Vector2.zero && _playerInput.runInput && _player != null && _player.CurrentStamina > 0)
                            {
                                PlayIdle("IdleRunEmpty");
                            }
                            else if (_playerInput.movementInput != Vector2.zero)
                            {
                                PlayIdle("IdleWalkEmpty");
                            }
                            else
                            {
                                PlayIdle("IdleEmpty");
                            }
                        }
                    }
                    break;
            }

        }

    }
    public void CanShoot()
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.SetCanShoot(true, _weaponType);
        }
    }

    public void CantShoot()
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.SetCanShoot(false, _weaponType);
        }
    }

    public void StartRealod()
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.StartReload();
        }
    }

    public void EndReload()
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.EndReload();
        }
    }

    public void UpdateAmmoText()
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.UpdateAmmoText();
        }
    }

    public void SetIdle(bool idle)
    {
        _playIdle = idle;
        _lastIdle = "";
    }

    public void SetIdleState(EIdleState state)
    {
        _idleState = state;
    }

    private void PlayIdle(string idle)
    {
        if (_animator != null)
        {
            if (_lastIdle != idle)
            {
                _animator.CrossFade(idle, 0.4f);
            }
            else
            {
                _animator.Play(idle);
            }

            _lastIdle = idle;
        }

    }

    public void CanIdle()
    {
        _playIdle = true;
    }


}



