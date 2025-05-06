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
    [SerializeField] private AudioClip _flashlightToggleSound;
    [SerializeField, Range(0f, 1f)] private float _flashlightToggleVolume;
    [SerializeField] private Color _highlight;
    [SerializeField] private Color _unhighlight;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Arrays ---
    [SerializeField] private WeaponData[] _weapons;
    [field: SerializeField] private List<Note> _notes = new List<Note>();

    // --- Note ---
    private GameObject _note;
    private GameObject _noteNext;
    private GameObject _notePrev;
    private int _noteIndex;

    // --- Weapon ---
    private WeaponData _defaultWeaponData; // If not needed remove

    // --- Util --- 
    private GameObject _item3DViwer; // If not needed remove
    private CursorController _cursor;
    private AudioSource _audioSource;

    // ---Slots---
    private GameObject _slot1;
    private GameObject _slot2;
    private GameObject _slot3;
    private GameObject _slot4;
    private GameObject _lastSlotUsed = null;

    // ---Flashlight---
    private bool _hasFlashlight = true;
    private GameObject _flashlight;
    private GameObject _flashlightLight;

    //--LastEquiped--
    private GameObject _lastEquiped = null;


    private void Awake()
    {
        _note = Util.FindSceneObjectByTag("Note");
        _noteNext = Util.FindSceneObjectByTag("NoteNext");
        _notePrev = Util.FindSceneObjectByTag("NotePrev");

        _item3DViwer = Util.FindSceneObjectByTag("Item3DViwer");
        _cursor = GetComponent<CursorController>();
        _audioSource = GetComponent<AudioSource>();

        _slot1 = Util.FindSceneObjectByTag("Slot1");
        _slot2 = Util.FindSceneObjectByTag("Slot2");
        _slot3 = Util.FindSceneObjectByTag("Slot3");
        _slot4 = Util.FindSceneObjectByTag("Slot4");

        _flashlight = Util.FindSceneObjectByTag("Flashlight");
        Light light = _flashlight.GetComponentInChildren<Light>();
        if(light != null)
        {
            _flashlightLight = light.gameObject;
        }
        
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

 //Journal & Notes start
    public void OpenJournal()
    {
        if (Util.IsNotNull(_note) && Util.IsNotNull(_cursor))
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
        if(Util.IsNotNull(_note) && _note.activeSelf)
        {

            if (_notes.Count <= 1)
            {
                SetNotesArrows(false);
            }
            else
            {
                SetNotesArrows(true);
            }

            if(_notes.Count == 1)
            {
                _noteIndex = 0;
                SetNoteText();
            }
        }
    }

    private void SetNoteText()
    {
        if (Util.IsNotNull(_note))
        {
            var noteText = _note.GetComponentInChildren<TextMeshProUGUI>(true);

            if (Util.IsNotNull(noteText))
            {
               
                if (_notes.Count > 0 && _noteIndex >= 0)
                {
                    noteText.SetText(_notes[_noteIndex]._description);

                    if(_notes.Count <= 1)
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

    private void SetNotesArrows(bool state)
    {
        if(Util.IsNotNull(_noteNext) && Util.IsNotNull(_notePrev))
        _noteNext.SetActive(state);
        _notePrev.SetActive(state);
    }
// Journal & Notes End

// Slot Higlight

    private void HiglightSlot(GameObject slot)
    {
        UnhighlightSlot();
        if(Util.IsNotNull(slot))
        {
            var text = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (Util.IsNotNull(text))
            {
                text.color = _highlight;
                _lastSlotUsed = slot;
            }
        } 
      
    }

    private void UnhighlightSlot()
    {
        if(_lastSlotUsed != null)
        {
            var text = _lastSlotUsed.GetComponentInChildren<TextMeshProUGUI>();
            if (Util.IsNotNull(text))
            {
                text.color = _unhighlight;
            }
            
        }
    }


// Slot End

// Flashlight

    private void PlayToggleFlashlightSound()
    {
       if (Util.IsNotNull(_audioSource) && Util.IsNotNull(_flashlightToggleSound))
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
        if(_hasFlashlight) 
        {
            if(Util.IsNotNull(_flashlight))
            {
               if(Util.ObjectToggle(_flashlight))
               {
                    UnEquip(_flashlight);
                    HiglightSlot(_slot2);
                    if(Util.IsNotNull(_flashlightLight))
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


   private void UnEquip(GameObject newEquip)
    {
        if (_lastEquiped != null && _lastEquiped != newEquip)
        {
            _lastEquiped.SetActive(false);

            if(_cursor.IsVisiable())
            {
                _cursor.DisableCursor();
            }
        }

        _lastEquiped = newEquip;

    }


    public void LMB()
    {
        if(_flashlight.activeInHierarchy)
        {
            if(_flashlightLight != null)
            {
                Util.ObjectToggle(_flashlightLight);
                PlayToggleFlashlightSound();
            }
        }
    }
}