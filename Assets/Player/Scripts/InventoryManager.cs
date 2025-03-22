using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public class InventoryManager : MonoBehaviour
{
    private WeaponData _defaultWeaponData;

    [SerializeField] private WeaponData[] _Weapons;
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


}
