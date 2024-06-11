using UnityEngine;

public class CombatModel
{
    public enum WeaponType
    {
        None,
        Fist,
        Trumpet
        //, Wip
    }

    private WeaponType currentWeapon;
    private float FistForce = 1.0f;
    private float trumpetForce = 5.0f;
// needs to me adjusted to work for multiple weapons.
    private int damageValue = 50;
    
    // some more weapons need to be added here

    //private float WipForce = 2.0f;

    public CombatModel()
    {
        currentWeapon = WeaponType.None;
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
        return damageValue;
    }

    public float GetAttackForce()
    {
        switch (currentWeapon)
        {
            case WeaponType.Fist:
                return FistForce;
            case WeaponType.Trumpet:
                return trumpetForce;
            default:
                return 0;
        }
    }
}
