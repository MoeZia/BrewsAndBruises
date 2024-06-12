using UnityEngine;
using System.Collections.Generic; // This is required for using Dictionary

public class CombatModel
{
    public enum WeaponType
    {
        None,
        Fist,
        Trumpet,
        Whip
    }

    private WeaponType currentWeapon;

    // Structure to hold weapon data
    public struct WeaponData {
        public float force;
        public int damage;

        public WeaponData(float force, int damage) {
            this.force = force;
            this.damage = damage;
        }
    }

    // Dictionary to hold weapon configurations
    private Dictionary<WeaponType, WeaponData> weaponsData = new Dictionary<WeaponType, WeaponData>();

    public CombatModel()
    {
        currentWeapon = WeaponType.None;
        InitializeWeapons();
    }

    private void InitializeWeapons() {
        // Initialize each weapon with its specific force and damage
        weaponsData.Add(WeaponType.Fist, new WeaponData(10.0f, 10));
        weaponsData.Add(WeaponType.Trumpet, new WeaponData(5.0f, 50));
        weaponsData.Add(WeaponType.Whip, new WeaponData(2.0f, 20));
    }

    public void SetWeapon(WeaponType weapon)
    {
        currentWeapon = weapon;
    }

    public WeaponType GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public int GetDamage(){
        if (weaponsData.ContainsKey(currentWeapon)) {
            return weaponsData[currentWeapon].damage;
        }
        return 0;
    }

    public float GetAttackForce()
    {
        if (weaponsData.ContainsKey(currentWeapon)) {
            return weaponsData[currentWeapon].force;
        }
        return 0;
    }
}
