using UnityEngine;
using System.Linq;

public class ShootingController : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private Transform _weaponCamera;
    [SerializeField] private GameObject _bulletHole;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _emptyMagazineSound;
    [SerializeField] private PlayerInput _playerInput;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Private Variables ---
    private bool _shootInput;
    private bool _canShoot;
    private bool _shotFired;
    private int _damage;
    private int _animationIndex;
    private float _shootingDistance; // Maximum distance the bullet can travel.
    private float _shootCooldownTime; // Time delay between shots.
    private float _shootCooldownTimer;

    private InventoryManager _inventoryManager;
    private GameObject _equipedWeapon;
    private WeaponData _weaponData;
    private AudioClip _shootingSound;
    private Animator[] _animators;
    private RaycastHit[] _hits;


    private void Awake()
    {
        _inventoryManager = gameObject.GetComponent<InventoryManager>();
        _shootCooldownTimer = _shootCooldownTime;
    }

    private void Update()
    {
        _shootInput = GetShootInput();
        HandleShootingCooldown();

        if (_shootInput && _shotFired && _equipedWeapon != null)
        {
            PlayShootingAnimation();

            if (_weaponData.ammo > 0)
            {
                _weaponData.ammo--;
                PlayShootingSound();
                HandleHits();
            }
            else
            {
                PlayEmptyMagazineSound();
            }

            _shotFired = false;

        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(EWeaponType.Pistol);
        }
    }

    private void FixedUpdate()
    {
        HandleShooting();
    }


    private bool GetShootInput()
    {
        if (_playerInput != null)
        {
            return _playerInput.shootInput;
        }
        return false;
    }

    private void HandleShootingCooldown()
    {
        if (!_canShoot)
        {
            _shootCooldownTimer -= Time.deltaTime;

            if (_shootCooldownTimer < 0)
            {
                _shootCooldownTimer = _shootCooldownTime;
                _canShoot = true;
            }

        }
    }


    private void PlayShootingSound()
    {
        if (_shootingSound != null)
        {
            _audioSource.PlayOneShot(_shootingSound);
        }
    }

    private void PlayEmptyMagazineSound()
    {
        if (_emptyMagazineSound != null)
        {
            _audioSource.PlayOneShot(_emptyMagazineSound);
        }
    }

    private void PlayShootingAnimation()
    {
        if (_animators.Length > 0)
        {
            _animators[_animationIndex].CrossFadeInFixedTime("Shot", 0.1f);
            _animationIndex++;
        }

        if (_animationIndex >= _animators.Length)
        {
            _animationIndex = 0;
        }
    }

    private void HandleShooting()
    {
        if (_equipedWeapon != null && _shootInput && _canShoot && _weaponData.ammo > 0)
        {
            _canShoot = false;
            _hits = Physics.RaycastAll(_weaponCamera.transform.position, _weaponCamera.transform.forward, _shootingDistance);
            _hits = _hits.OrderBy(hit => hit.distance).ToArray();
            _shotFired = true;
        }
    }

    private void HandleHits()
    {
        for (int i = 0; i < _hits.Length; i++)
        {
            if (_hits[i].collider == null)
            {
                continue;
            }

            if (_hits[i].collider.gameObject.transform.root.CompareTag("Enemy"))
            {
                Debug.Log("Hit");
                var hpScrip = _hits[i].collider.gameObject.GetComponentInParent<EnemyHealthManager>();
                if (hpScrip != null)
                {
                    // You have to pass the weapon dmg, the tag of the body part you hit by tag and the gameobject with the damage came from.
                    // hpScrip.TakeDamage(_damage, gameObject);
                }
                else
                {
                    Destroy(_hits[i].collider.gameObject);
                }

                break;
            }

            if (_hits[i].collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
            {
                if (_bulletHole != null)
                {
                    Vector3 vec = ((_weaponCamera.transform.position - _hits[i].point).normalized) * 0.001f; // points thowards weponCamera

                    Instantiate(_bulletHole, _hits[i].point + vec, Quaternion.FromToRotation(Vector3.up, _hits[i].normal));
                    break;
                }

            }

        }
    }

    private void DestroyEquipedWeapon()
    {
        if (_equipedWeapon != null)
        {
            Destroy(_equipedWeapon);
        }
    }

    private void EquipWeapon(EWeaponType weapon)
    {
        if (_inventoryManager == null)
        {
            return;
        }

        if (_weaponData != null && _weaponData.weaponType == weapon)
        {
            return;
        }

        WeaponData weaponData = _inventoryManager.GetWeapon(weapon);

        if (!weapon.Equals(default(WeaponData)) && weaponData.weaponPrefab != null && weaponData.acquired)
        {
            DestroyEquipedWeapon();

            _weaponData = weaponData;
            _equipedWeapon = Instantiate(weaponData.weaponPrefab, _weaponCamera.transform);
            var wepProperties = _equipedWeapon.GetComponent<WeaponProperties>();
            if (wepProperties != null)
            {
                _shootingSound = wepProperties.GetAudioClip();
                _animators = wepProperties.GetAnimators();
                _shootingDistance = wepProperties.GetShootDistance();
                _shootCooldownTime = wepProperties.GetShootCooldownTime();
                _damage = wepProperties.GetDamage();
            }
        }
    }
}