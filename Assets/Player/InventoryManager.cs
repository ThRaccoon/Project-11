using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


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
    public GameObject weaponPrefab;
    public EWeaponType weaponType;
    public bool acquired;
    public int ammoOnFound;
    public int ammo;
    public int ammoInMagazine;
    public int maxAmmo;
}

public class Item
{
    public int id;
    public string name;
    public string description;

}

public class Note
{
    public Note(string description) { _description = description; }
    public string _description;
}


public class InventoryManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Sounds")]
    [SerializeField] private AudioClip _pickUpNoteSound;
    [SerializeField, Range(0f, 1f)] private float _pickUpNoteVolume;
    [SerializeField] private AudioClip _journalSound;
    [SerializeField, Range(0f, 1f)] private float _journalVolume;
    [SerializeField] private AudioClip _flashlightToggleSound;
    [SerializeField, Range(0f, 1f)] private float _flashlightToggleVolume;
    [Header("Highlight Colors")]
    [SerializeField] private Color _highlight;
    [SerializeField] private Color _unhighlight;
    [Header("Note From Camera -> Canvas")]
    [SerializeField] private GameObject _note;
    [SerializeField] private GameObject _noteNext;
    [SerializeField] private GameObject _notePrev;
    [Header("Flashlight From Camera")]
    [SerializeField] private GameObject _flashlight;
    [SerializeField] private GameObject _flashlightLight;
    [Header("Slot From Camera -> Canvas")]
    [SerializeField] private GameObject _slot1;
    [SerializeField] private GameObject _slot2;
    [SerializeField] private GameObject _slot3;
    [Header("Cursor Script From Player")]
    [SerializeField] private CursorController _cursor;
    [Header("Audio Source From Player")]
    [SerializeField] private AudioSource _audioSource;
    [Header("Pistol/ShotGun From Camera")]
    [SerializeField] private WeaponData[] _weapons;
    // ----------------------------------------------------------------------------------------------------------------------------------    

    //--- Weapon ---
    private int _lastUsedWeaponIndex = -1;

    // --- Note ---
    private int _noteIndex;
    [field: SerializeField] private List<Note> _notes = new List<Note>();

    // --- Util --- 
    private GameObject _item3DViwer; // If not needed remove


    // ---Slots---    
    private GameObject _slot4;
    private GameObject _lastSlotUsed = null;

    // ---Flashlight---
    private bool _hasFlashlight = true;


    //--LastEquiped--
    private GameObject _lastEquiped = null;


    private void Awake()
    {

    }


    //Journal & Notes start
    public void OpenJournal()
    {
        if (_note != null && _cursor != null)
        {
            if (Util.ObjectToggle(_note))
            {
                UnEquip(_note);
                HiglightSlot(_slot3);
                _cursor.EnableCursor();
                _noteIndex = _notes.Count - 1;
                SetNoteText();

            }
            else
            {
                UnhighlightSlot();
                _cursor.DisableCursor();
            }

            PlayJournalSound();
        }
    }

    public void AddNote(string description)
    {
        Note note = new Note(description);
        _notes.Add(note);
        PlayPickUpNoteSound();
        UpdateJournal();
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

    public void NextNote()
    {
        if (_notes.Count > 1)
        {
            _noteIndex = (_noteIndex + 1) % _notes.Count;
            SetNoteText();
            PlayJournalSound();
        }
    }

    public void PreviousNote()
    {
        if (_notes.Count > 1)
        {
            _noteIndex = (_noteIndex - 1) < 0 ? _notes.Count - 1 : (_noteIndex - 1);
            SetNoteText();
            PlayJournalSound();
        }
    }

    private void PlayJournalSound()
    {
        if (_audioSource != null && _pickUpNoteSound != null)
        {
            _audioSource.clip = _journalSound;
            _audioSource.volume = _journalVolume;
            _audioSource.Play();
        }
    }

    private void PlayPickUpNoteSound()
    {
        if (_audioSource != null && _pickUpNoteSound != null)
        {
            _audioSource.clip = _pickUpNoteSound;
            _audioSource.volume = _pickUpNoteVolume;
            _audioSource.Play();
        }
    }

    private void SetNotesArrows(bool state)
    {
        if (_noteNext != null && _notePrev != null)
            _noteNext.SetActive(state);
        _notePrev.SetActive(state);
    }
    // Journal & Notes End

    // Slot Higlight

    private void HiglightSlot(GameObject slot)
    {
        UnhighlightSlot();
        if (slot != null)
        {
            var text = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.color = _highlight;
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
                text.color = _unhighlight;
            }

        }
    }


    // Slot Higlight End

    // Flashlight

    private void PlayToggleFlashlightSound()
    {
        if (_audioSource != null && _flashlightToggleSound != null)
        {
            _audioSource.clip = _flashlightToggleSound;
            _audioSource.volume = _flashlightToggleVolume;
            _audioSource.Play();
        }

    }
    public void SetHasFlashlight(bool state)
    {
        _hasFlashlight = state;
    }

    public void EquipFlashlight()
    {
        if (_hasFlashlight)
        {
            if (_flashlight != null)
            {
                if (Util.ObjectToggle(_flashlight))
                {
                    UnEquip(_flashlight);
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

    // Flashlight End

    // Weapon
    public WeaponData GetWeapon(EWeaponType weapon)
    {
        WeaponData foundWeapon = Array.Find(_weapons, w => w.weaponType == weapon);
        return foundWeapon;
    }

    public bool IsFullOfAmmo(EWeaponType weapon)
    {
        WeaponData foundWeapon = Array.Find(_weapons, w => w.weaponType == weapon);

        if (!foundWeapon.Equals(default(WeaponData)))
        {
            return foundWeapon.ammo == foundWeapon.maxAmmo;
        }
        return false;
    }

    public void OnWeaponFound(EWeaponType weapon)
    {
        WeaponData foundWeapon = Array.Find(_weapons, w => w.weaponType == weapon);

        if (!foundWeapon.Equals(default(WeaponData)))
        {
            foundWeapon.acquired = true;
            OnAmmoFound(foundWeapon.weaponType, foundWeapon.ammoOnFound);
        }
    }

    public void OnAmmoFound(EWeaponType weapon, int amountAmmo)
    {
        WeaponData foundWeapon = Array.Find(_weapons, w => w.weaponType == weapon);

        if (!foundWeapon.Equals(default(WeaponData)))
        {
            foundWeapon.ammo = Math.Clamp(foundWeapon.ammo + amountAmmo, 0, foundWeapon.maxAmmo);
        }
    }
    public void EquipWeapon()
    {
        if (_lastUsedWeaponIndex == -1)
        {
            for (int i = 0; i < _weapons.Length; i++)
            {
                if (_weapons[i].acquired)
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
                UnEquip(_weapons[_lastUsedWeaponIndex].weaponPrefab);
                HiglightSlot(_slot1);
            }
            else
            {
                UnhighlightSlot();
            }
        }
    }

    // Weapon End
    private void UnEquip(GameObject newEquip)
    {
        if (_lastEquiped != null && _lastEquiped != newEquip)
        {
            _lastEquiped.SetActive(false);

            if (_cursor.IsVisiable())
            {
                _cursor.DisableCursor();
            }
        }

        _lastEquiped = newEquip;

    }


    public void LMB()
    {
        if (_flashlight.activeInHierarchy)
        {
            if (_flashlightLight != null)
            {
                Util.ObjectToggle(_flashlightLight);
                PlayToggleFlashlightSound();
            }
        }
    }
}