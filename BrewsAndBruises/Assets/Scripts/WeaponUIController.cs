using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIController : MonoBehaviour
{
    private CombatController player;

    private CombatModel.WeaponType currentWeapon;
    void Start() {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<CombatController>(); 
    }

    
    void Update() {
        if(player.GetCurrentWeapon() != CombatModel.WeaponType.None) {
            currentWeapon = player.GetCurrentWeapon();
            GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");

            foreach (GameObject weapon in weapons){
                WeaponUI ui = weapon.GetComponent<WeaponUI>();
                ui.setTexture(0);
                if(currentWeapon == CombatModel.WeaponType.Mug && ui.weaponType == CombatModel.WeaponType.Mug ) {
                    weapon.GetComponent<WeaponUI>().setTexture(1);
                }

                if(currentWeapon == CombatModel.WeaponType.Breze && ui.weaponType == CombatModel.WeaponType.Breze) {
                    weapon.GetComponent<WeaponUI>().setTexture(1);
                }

                if(currentWeapon == CombatModel.WeaponType.Trumpet && ui.weaponType == CombatModel.WeaponType.Trumpet) {
                    weapon.GetComponent<WeaponUI>().setTexture(1);
                }
            }
        }
        
        
    }
}
