using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.ComponentModel.Design;

public class ShootingController : MonoBehaviour
{
    // --- Components ---
    [Header("Components")]
    [SerializeField] private Transform _WeaponCamera = null;
    [SerializeField] private PlayerInput _PlayerInput = null;
    [SerializeField] private AudioSource _AudioSource = null;
    [SerializeField] private GameObject _BulletHole = null;
    [SerializeField] private AudioClip _EmptyMagazineSound = null;

    // --- Private Variables ---
    private int _animationIndex = 0;
    private float _shootCooldownTimer = 0.0f;
    private bool _shootInput = false;
    private bool _canShoot = false;
    private RaycastHit[] _hits;
    private AudioClip _ShootingSound = null;
    private Animator[] _Animators = null;
    private float _shootingDistance = 100f; // Maximum distance the bullet can travel.
    private float _shootCooldownTime = 0.5f;// Time delay between shots.
    private int _damage = 25;
    private bool _shootFired = false;
    private InventoryManager _InventoryManager = null;
    private GameObject _EquipedWeapon;
    private WeaponData _WeaponData = null;
    
    
    
    private void Awake()
    {
        _InventoryManager = gameObject.GetComponent<InventoryManager>();
        _shootCooldownTimer = _shootCooldownTime;
    }

    private void Update()
    {
        _shootInput = GetShootInput();
        HandleShootingCooldown();

        if (_shootInput && _shootFired && _EquipedWeapon != null)
        {
            PlayShootingAnimation();

            if (_WeaponData.ammo > 0) 
            {
                _WeaponData.ammo--;
                PlayShootingSound();
                HandleHits();
            }
            else
            {
                PlayEmptyMagazineSound();
            }

            _shootFired = false;

        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(EWeaponType.Pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(EWeaponType.MP5);
        }

    }

    private void FixedUpdate()
    {
        HandleShooting();
    }
    private bool GetShootInput()
    {
        return _PlayerInput.ShootInput;
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
        if (_ShootingSound != null)
        {
            _AudioSource.PlayOneShot(_ShootingSound);
        }
    }

    private void PlayEmptyMagazineSound()
    {
        if (_EmptyMagazineSound != null)
        {
            _AudioSource.PlayOneShot(_EmptyMagazineSound);
        }
    }


    private void PlayShootingAnimation()
    {
        if (_Animators.Length > 0)
        {
            _Animators[_animationIndex].CrossFadeInFixedTime("Shot", 0.1f);
            _animationIndex++;
        }

        if (_animationIndex >= _Animators.Length)
        {
            _animationIndex = 0;
        }
    }
    private void HandleShooting()
    {
        if (_shootInput && _canShoot && _EquipedWeapon != null && _WeaponData.ammo>0)
        {
            _canShoot = false;
            _hits = Physics.RaycastAll(_WeaponCamera.transform.position, _WeaponCamera.transform.forward, _shootingDistance);
            _hits = _hits.OrderBy(hit => hit.distance).ToArray();
            _shootFired = true;
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
                var hpScrip = _hits[i].collider.gameObject.GetComponentInParent<Health>();
                if (hpScrip != null)
                {
                    hpScrip.TakeDamage(_damage, gameObject);
                }
                else
                {
                    Destroy(_hits[i].collider.gameObject);
                }

                break;
            }

            if (_hits[i].collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
            {
                if (_BulletHole != null)
                {
                    Vector3 vec = ((_WeaponCamera.transform.position - _hits[i].point).normalized) * 0.001f; // points thowards weponCamera

                    Instantiate(_BulletHole, _hits[i].point + vec, Quaternion.FromToRotation(Vector3.up, _hits[i].normal));
                    break;
                }

            }

        }
    }

    private void DestroyEquipedWeapon()
    {
        if (_EquipedWeapon != null)
        {
            Destroy(_EquipedWeapon);
        }
    }

    private void EquipWeapon(EWeaponType weapon)
    {
        if (_InventoryManager == null)
        {
            return;
        }

        if (_WeaponData != null && _WeaponData.weaponType ==  weapon)
        {
            return;
        }

        WeaponData weaponData = _InventoryManager.GetWeapon(weapon);

        if (!weapon.Equals(default(WeaponData)) && weaponData.weaponPrefab != null &&  weaponData.acquired)
        {
            DestroyEquipedWeapon();

            _WeaponData = weaponData;
            _EquipedWeapon = Instantiate(weaponData.weaponPrefab, _WeaponCamera.transform);
            var wepProperties = _EquipedWeapon.GetComponent<WeaponProperties>();
            if (wepProperties != null)
            {
                _ShootingSound = wepProperties.GetAudioClip();
                _Animators = wepProperties.GetAnimators();
                _shootingDistance = wepProperties.GetShootDistance();
                _shootCooldownTime = wepProperties.GetShootCooldownTime();
                _damage = wepProperties.GetDamage();
            }
        }
    }
}
