using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour {
    public UnityEvent<CombatModel.WeaponType> OnWeaponChange = new UnityEvent<CombatModel.WeaponType>();
    public UnityEvent OnAttack = new UnityEvent();

    public Transform weaponsParent; // Assign this to Elbow_L in the Unity Inspector

    private CombatModel combatModel;
    private Dictionary<CombatModel.WeaponType, GameObject> weaponObjects = new Dictionary<CombatModel.WeaponType, GameObject>();

    void Start() {
        combatModel = new CombatModel();
        InitializeWeaponObjects();
        InitializeEvents();
    }

    private void InitializeWeaponObjects() {
        // Assuming the names of the child GameObjects are "Fist", "Trumpet", "Whip"
        weaponObjects[CombatModel.WeaponType.Mug] = weaponsParent.Find("Mug").gameObject;
        weaponObjects[CombatModel.WeaponType.Breze] = weaponsParent.Find("Breze").gameObject;
        weaponObjects[CombatModel.WeaponType.Whip] = weaponsParent.Find("Whip").gameObject;

        // Set all weapons inactive initially
        foreach (var weapon in weaponObjects.Values) {
           // weapon.SetActive(false);
           // SphereCollider collider = weapon.GetComponent<SphereCollider>();
           // if (collider == null) {
           //     collider = weapon.AddComponent<SphereCollider>();
           //     collider.radius = 0.05f; // Set the radius as needed
           // }
           // collider.isTrigger = true;
        }
    }

    private void InitializeEvents() {
        OnWeaponChange.AddListener(HandleWeaponChange);
        OnAttack.AddListener(PerformAttack);
    }

    public void HandleWeaponChange(CombatModel.WeaponType weapon) {
        combatModel.SetWeapon(weapon);
        UpdateWeaponVisibility(weapon);
    }

    private void UpdateWeaponVisibility(CombatModel.WeaponType weapon) {
        foreach (var wp in weaponObjects) {
            wp.Value.SetActive(wp.Key == weapon);
        }
    }

    public CombatModel.WeaponType GetCurrentWeapon() {
        return combatModel.GetCurrentWeapon();
    }

    public void PerformAttack() {
    CombatModel.WeaponType currentWeapon = GetCurrentWeapon();
    GameObject activeWeapon = weaponObjects[currentWeapon];
    activeWeapon.SetActive(true);  // Ensure the weapon is active

    // Define the radius for your attack range, adjust as necessary
    float attackRadius = 2.5f;
    Collider[] hitColliders = Physics.OverlapSphere(activeWeapon.transform.position, attackRadius);

    foreach (var hitCollider in hitColliders) {
        // Check if the collider is on the "Hittable" layer and tagged as "Enemy"
        if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Hittable") && hitCollider.CompareTag("Enemy")) {
            Health enemyHealth = hitCollider.GetComponent<Health>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(combatModel.GetDamage());
                Debug.Log($"Hit {hitCollider.name}: Dealing {combatModel.GetDamage()} damage.");
            }
            EnemyController enemy = hitCollider.GetComponent<EnemyController>();
            if (enemy != null) {
        Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
        float pushForce = combatModel.GetAttackForce(); // Assuming this returns the force based on the weapon
        enemy.ApplyPushback(pushDirection * pushForce);
    }
        }
       
    }

   // Debug.Log($"Attacking with {currentWeapon} dealing {combatModel.GetDamage()} damage.");
}

}
