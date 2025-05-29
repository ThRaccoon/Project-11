using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum EWeaponType
{
    None = 0,
    Pistol = 1,
    ShotGun = 2
}

[System.Serializable]
public class WeaponData
{
    public Animator animator;
    public GameObject weaponPrefab;

    public EWeaponType weaponType;

    public bool isAcquired;

    public int reserveCapacity;
    public int magazineCapacity;
    public int ammoOnFound;
    public int ammoInReserve;
    public int ammoInMagazine;

    public int damage;
    public bool canShoot = true;
}

public class ItemData
{
    public int id;
    public string name;
    public string description;
}

public class NoteData
{
    public NoteData(string description) { _description = description; }
    public string _description;
}

public class InventoryManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private CursorController _cursorController;
    [SerializeField] private ShootingManager _shootingManager;
    [SerializeField] private WeaponAnimationManager _weaponAnimationManager;
    [SerializeField] private AudioSource _audioSource;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(15)]
    [Header("Weapon Related")]
    [SerializeField] private WeaponData[] _weapons;
    [SerializeField] private GameObject _ammoText;

    [SerializeField] private RawImage _pistolImage;
    [SerializeField] private RawImage _shotGunImage;

    [Header("Pistol From UI Folder")]
    [SerializeField] private Texture _pistolTextureSelected;
    [SerializeField] private Texture _pistolTextureUnselected;

    [Header("Shotgun From UI Folder")]
    [SerializeField] private Texture _shotGunTextureSelected;
    [SerializeField] private Texture _shotGunTextureUnselected;

    [SerializeField] private GameObject _slot1;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(15)]
    [Header("Flashlight Related")]
    [SerializeField] private GameObject _flashlightHand;
    [SerializeField] private GameObject _flashlightLight;

    [SerializeField] private AudioClip _flashlightToggleSound;
    [SerializeField, Range(0f, 1f)] private float _flashlightToggleVolume;

    [SerializeField] private GameObject _slot2;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(15)]
    [Header("Journal Related")]
    [SerializeField] private AudioClip _journalSound;
    [SerializeField, Range(0f, 1f)] private float _journalVolume;

    [SerializeField] private GameObject _slot3;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(15)]
    [Header("Note Related")]
    [SerializeField] private GameObject _note;
    [SerializeField] private GameObject _noteNext;
    [SerializeField] private GameObject _notePrev;

    [SerializeField] private AudioClip _pickUpNoteSound;
    [SerializeField, Range(0f, 1f)] private float _pickUpNoteVolume;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [field: Space(15)]
    [Header("Highlight Colors")]
    [SerializeField] private Color _highlighted;
    [SerializeField] private Color _unhighlighted;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // --- Weapon Related ---
    private GameObject _lastEquipped;
    private int _lastUsedWeaponIndex = -1;

    public WeaponData currentWeapon { get; private set; }

    // --- Flashlight Related ---
    private bool _hasFlashlight = true; // There was function called SetHasFlashlight this getter/setter is doing the same but you have to call it HasFlashlight = true / false or if (HasFlashlight)
    #region Getters / Setters

    public bool HasFlashlight
    {
        get => _hasFlashlight;
        set => _hasFlashlight = value;
    }
    #endregion              

    // --- Note Related ---
    [field: SerializeField] private List<NoteData> _notes = new List<NoteData>();
    private int _noteIndex;

    // --- Slot Related ---
    private GameObject _slot4;
    private GameObject _lastSlotUsed = null;


    // --- Weapon Related ---
    public WeaponData FindWeapon(EWeaponType weapon)
    {
        WeaponData foundWeapon = Array.Find(_weapons, w => w.weaponType == weapon);

        return foundWeapon;
    }

    public void OnWeaponFound(EWeaponType weapon)
    {
        WeaponData foundWeapon = Array.Find(_weapons, w => w.weaponType == weapon);

        if (!foundWeapon.Equals(default(WeaponData)))
        {
            foundWeapon.isAcquired = true;
            OnAmmoFound(foundWeapon.weaponType, foundWeapon.ammoOnFound);
        }
    }

    public void OnAmmoFound(EWeaponType weapon, int amountAmmo)
    {
        WeaponData foundWeapon = Array.Find(_weapons, w => w.weaponType == weapon);

        if (!foundWeapon.Equals(default(WeaponData)))
        {
            foundWeapon.ammoInReserve = Math.Clamp(foundWeapon.ammoInReserve + amountAmmo, 0, foundWeapon.reserveCapacity);
        }
    }


    public void EquipWeapon()
    {
        if (_lastUsedWeaponIndex == -1)
        {
            for (int i = 0; i < _weapons.Length; i++)
            {
                if (_weapons[i].isAcquired)
                {
                    _lastUsedWeaponIndex = i;
                    break;
                }
            }
        }

        if (_lastUsedWeaponIndex < 0 && _lastUsedWeaponIndex >= _weapons.Length)
        {
            return;
        }

        if (_weapons[_lastUsedWeaponIndex].weaponPrefab != null)
        {
            if (Util.ObjectToggle(_weapons[_lastUsedWeaponIndex].weaponPrefab))
            {
                UpdateEquipped(_weapons[_lastUsedWeaponIndex].weaponPrefab);
                HiglightSlot(_slot1);

                currentWeapon = _weapons[_lastUsedWeaponIndex];
                _weaponAnimationManager.OnWeaponEnabled(currentWeapon.animator);

                switch (_weapons[_lastUsedWeaponIndex].weaponType)
                {
                    case EWeaponType.Pistol:
                        {
                            if (_pistolImage != null)
                            {
                                _pistolImage.texture = _pistolTextureSelected;
                            }

                            if (_shotGunImage != null)
                            {
                                _shotGunImage.texture = _shotGunTextureUnselected;
                            }
                        }
                        break;

                    case EWeaponType.ShotGun:
                        {
                            if (_shotGunImage != null)
                            {
                                _shotGunImage.texture = _shotGunTextureSelected;
                            }

                            if (_pistolImage != null)
                            {
                                _pistolImage.texture = _pistolTextureUnselected;
                            }
                        }
                        break;
                }

                UpdateAmmoText();
            }
            else
            {
                UnhighlightSlot();
            }
        }
    }

    public void EquipNextWeapon()
    {
        if (_lastUsedWeaponIndex == -1)
        {
            return;
        }

        if (!_weapons[_lastUsedWeaponIndex].weaponPrefab.activeInHierarchy)
        {
            return;
        }

        int _nextWeaponIndex = (_lastUsedWeaponIndex + 1) % _weapons.Length;

        while (!_weapons[_nextWeaponIndex].isAcquired)
        {
            _nextWeaponIndex = (_nextWeaponIndex + 1) % _weapons.Length;
        }

        if (_nextWeaponIndex == _lastUsedWeaponIndex)
        {
            return;
        }

        _lastUsedWeaponIndex = _nextWeaponIndex;

        EquipWeapon();
    }


    public bool IsFullOfAmmo(EWeaponType weapon)
    {
        WeaponData foundWeapon = Array.Find(_weapons, w => w.weaponType == weapon);

        if (!foundWeapon.Equals(default(WeaponData)))
        {
            return foundWeapon.ammoInReserve == foundWeapon.reserveCapacity;
        }
        return false;
    }

    public void StartReload()
    {
        if (_lastUsedWeaponIndex >= 0 && _lastUsedWeaponIndex < _weapons.Length && _weapons[_lastUsedWeaponIndex].weaponPrefab.activeInHierarchy)
        {
            if (_weapons[_lastUsedWeaponIndex].ammoInMagazine < _weapons[_lastUsedWeaponIndex].magazineCapacity && _weapons[_lastUsedWeaponIndex].ammoInReserve > 0)
            {
                _weaponAnimationManager.ChangeState(WeaponAnimationManager.EWeaponState.Reload);
            }
        }
    }

    public void EndReload()
    {
        if (_lastUsedWeaponIndex >= 0 && _lastUsedWeaponIndex < _weapons.Length && _weapons[_lastUsedWeaponIndex].weaponPrefab.activeInHierarchy)
        {
            int neededAmmo = _weapons[_lastUsedWeaponIndex].magazineCapacity - _weapons[_lastUsedWeaponIndex].ammoInMagazine;
            int bulletsReloaded = Mathf.Min(neededAmmo, _weapons[_lastUsedWeaponIndex].ammoInReserve);

            _weapons[_lastUsedWeaponIndex].ammoInMagazine += bulletsReloaded;
            _weapons[_lastUsedWeaponIndex].ammoInReserve -= bulletsReloaded;

            UpdateAmmoText();
        }
    }

    public void UpdateAmmoText()
    {
        if (_ammoText != null)
        {
            _ammoText.SetActive(true);

            var text = _ammoText.GetComponentInChildren<TextMeshProUGUI>();

            if (text != null)
            {
                text.text = "Ammo " + _weapons[_lastUsedWeaponIndex].ammoInMagazine + " } " + _weapons[_lastUsedWeaponIndex].ammoInReserve;
            }
        }
    }


    public void SetCanShoot(bool canShoot, EWeaponType weapon)
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            if (_weapons[i].weaponType == weapon)
            {
                _weapons[i].canShoot = canShoot;
            }
        }
    }


    // --- Flashlight Related ---
    public void EquipFlashlight()
    {
        if (_hasFlashlight)
        {
            if (_flashlightHand != null)
            {
                if (Util.ObjectToggle(_flashlightHand))
                {
                    UpdateEquipped(_flashlightHand);

                    HiglightSlot(_slot2);

                    if (_flashlightLight != null)
                    {
                        _flashlightLight.SetActive(false);
                    }
                }
                else
                {
                    UnhighlightSlot();
                }
            }
        }
    }


    // --- Journal Related---  
    public void OpenJournal()
    {
        if (_note != null && _cursorController != null)
        {
            if (Util.ObjectToggle(_note))
            {
                UpdateEquipped(_note);
                HiglightSlot(_slot3);

                _cursorController.EnableCursor();
                _noteIndex = _notes.Count - 1;

                SetNoteText();

            }
            else
            {
                UnhighlightSlot();

                _cursorController.DisableCursor();
            }

            PlaySound(_journalSound, _journalVolume);
        }
    }

    private void UpdateJournal()
    {
        if (_note != null && _note.activeSelf)
        {
            if (_notes.Count <= 1)
            {
                SetNotesArrows(false);
            }
            else
            {
                SetNotesArrows(true);
            }

            if (_notes.Count == 1)
            {
                _noteIndex = 0;

                SetNoteText();
            }
        }
    }


    // --- Note Related---  
    public void AddNote(string description)
    {
        NoteData note = new NoteData(description);
        _notes.Add(note);

        PlaySound(_pickUpNoteSound, _pickUpNoteVolume);

        UpdateJournal();
    }


    public void NextNote()
    {
        if (_notes.Count > 1)
        {
            _noteIndex = (_noteIndex + 1) % _notes.Count;

            SetNoteText();

            PlaySound(_journalSound, _journalVolume);
        }
    }

    public void PreviousNote()
    {
        if (_notes.Count > 1)
        {
            _noteIndex = (_noteIndex - 1) < 0 ? _notes.Count - 1 : (_noteIndex - 1);

            SetNoteText();

            PlaySound(_journalSound, _journalVolume);
        }
    }


    private void SetNoteText()
    {
        if (_note != null)
        {
            var noteText = _note.GetComponentInChildren<TextMeshProUGUI>(true);

            if (noteText != null)
            {

                if (_notes.Count > 0 && _noteIndex >= 0)
                {
                    noteText.SetText(_notes[_noteIndex]._description);

                    if (_notes.Count <= 1)
                    {
                        SetNotesArrows(false);
                    }
                    else
                    {
                        SetNotesArrows(true);
                    }
                }
                else
                {
                    noteText.SetText("EMPTY");

                    SetNotesArrows(false);
                }
            }
        }
    }

    private void SetNotesArrows(bool state)
    {
        if (_noteNext != null && _notePrev != null)
        {
            _noteNext.SetActive(state);
        }

        _notePrev.SetActive(state);
    }


    // --- Slote Related---
    private void HiglightSlot(GameObject slot)
    {
        UnhighlightSlot();
        if (slot != null)
        {
            var text = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.color = _highlighted;
                _lastSlotUsed = slot;
            }
        }
    }

    private void UnhighlightSlot()
    {
        if (_lastSlotUsed != null)
        {
            var text = _lastSlotUsed.GetComponentInChildren<TextMeshProUGUI>();

            if (text != null)
            {
                text.color = _unhighlighted;
            }
        }

        //Ammo text
        if (_ammoText != null)
        {
            _ammoText.SetActive(false);
        }
    }


    // --- Generic ---
    public void LMB()
    {
        if (_flashlightHand.activeInHierarchy)
        {
            if (_flashlightLight != null)
            {
                Util.ObjectToggle(_flashlightLight);
                PlaySound(_flashlightToggleSound, _flashlightToggleVolume);
            }
        }
        else if (_lastUsedWeaponIndex >= 0 && _lastUsedWeaponIndex < _weapons.Length)
        {
            if (_shootingManager != null && _weaponAnimationManager != null && _weapons[_lastUsedWeaponIndex].weaponPrefab.activeInHierarchy && _weapons[_lastUsedWeaponIndex].canShoot)
            {
                if (_weapons[_lastUsedWeaponIndex].ammoInMagazine > 0)
                {
                    _shootingManager.Shoot(_weapons[_lastUsedWeaponIndex].damage);
                    _weapons[_lastUsedWeaponIndex].ammoInMagazine--;

                    UpdateAmmoText();

                    _weaponAnimationManager.ChangeState(WeaponAnimationManager.EWeaponState.Shoot);
                }
            }
        }
    }


    private void UpdateEquipped(GameObject newEquipped)
    {
        if (_lastEquipped != null && _lastEquipped != newEquipped)
        {
            _lastEquipped.SetActive(false);

            if (_cursorController.IsVisiable())
            {
                _cursorController.DisableCursor();
            }
        }

        _lastEquipped = newEquipped;
    }


    // --- Utility ---
    private void PlaySound(AudioClip audioClip, float volume)
    {
        if (_audioSource != null && audioClip != null)
        {
            _audioSource.clip = audioClip;
            _audioSource.volume = volume;
            _audioSource.Play();
        }
    }
}