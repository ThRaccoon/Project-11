using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public enum EWeaponType
{
    None = 0,
    Pistol = 1,
}

[System.Serializable]
public class WeaponData
{
    public GameObject weaponPrefab;
    public EWeaponType weaponType;
    public bool acquired;
    public int ammoOnFound;
    public int ammo;
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
    [Header("Components")]
    [SerializeField] private AudioClip _pickUpNoteSound;
    [SerializeField, Range(0f, 1f)] private float _pickUpNoteVolume;
    [SerializeField] private AudioClip _journalSound;
    [SerializeField, Range(0f, 1f)] private float _journalVolume;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Arrays ---
    [SerializeField] private WeaponData[] _weapons;
    [field: SerializeField] private List<Note> _notes = new List<Note>();

    // --- Note ---
    private GameObject _note;
    private int _noteIndex;

    // --- Weapon ---
    private WeaponData _defaultWeaponData; // If not needed remove

    // --- Util --- 
    private GameObject _item3DViwer; // If not needed remove
    private CursorController _cursor;
    private AudioSource _audioSource;


    private void Awake()
    {
        _note = Util.FindSceneObjectByTag("Note");
        _item3DViwer = Util.FindSceneObjectByTag("Item3DViwer");
        _cursor = GetComponent<CursorController>();
        _audioSource = GetComponent<AudioSource>();
    }


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


    public void OpenJournal()
    {
        if (Util.IsNotNull(_note) && Util.IsNotNull(_cursor))
        {
            if (Util.ObjectToggle(_note))
            {
                _cursor.EnableCursor();
                _noteIndex = _notes.Count - 1;
                SetNoteText();
            }
            else
            {
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
    }

    private void SetNoteText()
    {
        var noteText = _note.GetComponentInChildren<TextMeshProUGUI>(true);

        if (Util.IsNotNull(noteText))
        {
            if (_notes.Count > 0 && _noteIndex >= 0)
            {
                noteText.SetText(_notes[_noteIndex]._description);
            }
            else
            {
                noteText.SetText("EMPTY");
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
        if (Util.IsNotNull(_audioSource) && Util.IsNotNull(_pickUpNoteSound))
        {
            _audioSource.clip = _journalSound;
            _audioSource.volume = _journalVolume;
            _audioSource.Play();
        }
    }

    private void PlayPickUpNoteSound()
    {
        if (Util.IsNotNull(_audioSource) && Util.IsNotNull(_pickUpNoteSound))
        {
            _audioSource.clip = _pickUpNoteSound;
            _audioSource.volume = _pickUpNoteVolume;
            _audioSource.Play();
        }
    }
}