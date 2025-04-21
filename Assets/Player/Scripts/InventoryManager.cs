using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



[System.Serializable]
public enum EWeaponType
{
    None = 0,
    Pistol = 1,
    MP5 = 2,
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
 
    ///////////////////////////

    private WeaponData _defaultWeaponData;
    private GameObject _note;
    private int _noteIndex;
    private GameObject _item3DViwer;
    private CursorController _cursor;

    private void Awake()
    {
        _note = Utility.FindSceneObjectByTag("Note");
        _item3DViwer = Utility.FindSceneObjectByTag("Item3DViwer");
        var player = Utility.FindSceneObjectByTag("Player");

        if(player != null )
        {
            _cursor = player.GetComponent<CursorController>();
        }
       
    }

    [SerializeField] private WeaponData[] _Weapons;
    [field:SerializeField] private List<Note> _Notes = new List<Note>();
    //////////////////////////////////////////////
    //public functions
    //////////////////////////////////////////////
    public WeaponData GetWeapon(EWeaponType weapon)
    {
        WeaponData foundWeapon = Array.Find(_Weapons, w => w.weaponType == weapon);
        return foundWeapon;
    }

    public bool IsFullOfAmmo(EWeaponType weapon)
    {
        WeaponData foundWeapon = Array.Find(_Weapons, w => w.weaponType == weapon);
        if(!foundWeapon.Equals(default(WeaponData)))
        {
            return foundWeapon.ammo == foundWeapon.maxAmmo;
        }
        return false;
    }

    public void OnWeaponFound(EWeaponType weapon)
    {
        WeaponData foundWeapon = Array.Find(_Weapons, w => w.weaponType == weapon);
        if (!foundWeapon.Equals(default(WeaponData)))
        {
            foundWeapon.acquired = true;
            OnAmmoFound(foundWeapon.weaponType, foundWeapon.ammoOnFound);
        }
    }

    public void OnAmmoFound(EWeaponType weapon, int amountAmmo)
    {
        WeaponData foundWeapon = Array.Find(_Weapons, w => w.weaponType == weapon);
        if (!foundWeapon.Equals(default(WeaponData)))
        {
            foundWeapon.ammo = Math.Clamp(foundWeapon.ammo+amountAmmo, 0, foundWeapon.maxAmmo);
        }
    }


    public void AddNote(string description)
    {
        Note note = new Note(description);
       _Notes.Add(note);
    }

    public void OpenJournal()
    {
         if(_note != null && _cursor)
        {

            if (Utility.ObjectToggle(_note))
            {
                _cursor.EnableCursor();
                _noteIndex = _Notes.Count - 1;
                SetNotText();
            }
            else
            {
                _cursor.DisableCursor();
            }
        }
    }

    public void NextNote()
    {
        if(_Notes.Count > 0)
        {
            _noteIndex = (_noteIndex + 1) % _Notes.Count;
            SetNotText();
        }
        
    } 
    
    public void PrevNote()
    {
        if (_Notes.Count > 0)
        {
            _noteIndex = (_noteIndex + 1) % _Notes.Count;
            SetNotText();
        }
    }

    private void SetNotText()
    {
        var noteText = _note.GetComponentInChildren<TextMeshProUGUI>(true);
        if (noteText != null) 
        {
            if (_Notes.Count > 0 && _noteIndex >= 0)
            {
                noteText.SetText(_Notes[_noteIndex]._description);
            }
            else
            {
                noteText.SetText("EMPTY");
            }
        }
       
        
       
    }



}
