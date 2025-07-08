using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

// SO để lưu dữ liệu
[CreateAssetMenu(fileName = "WeaponDataBase", menuName = "Weapons/Weapon Database")]
public class WeaponDataBase : ScriptableObject
{
    public List<WeaponData> WeaponDatas = new List<WeaponData>();

}

[System.Serializable]
public class WeaponData
{
    public string weaponName, pathIcon;
    public int damage,ammo;
    public float dispersion, rateOfFire, reloadSpeed;
    [JsonIgnore] public Sprite weaponIcon;
    public WeaponStatus status = WeaponStatus.None;

    // Clone wpData
    public WeaponData Clone()
    {
        return new WeaponData
        {
            weaponName = this.weaponName,
            weaponIcon = this.weaponIcon,
            pathIcon = this.pathIcon,
            damage = this.damage,
            dispersion = this.dispersion,
            rateOfFire = this.rateOfFire,
            reloadSpeed = this.reloadSpeed,
            ammo = this.ammo,
            status = this.status
        };
    }

    //Tăng chỉ số khi nâng cấp
    public void Upgrade()
    {
        damage += 1;
        ammo += 1;
    }
}

// Vũ khí có 3 trang thái.
public enum WeaponStatus
{
    None,
    Used,
    RentedOut
}