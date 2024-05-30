using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour
{
    public UnityEvent<CombatModel.WeaponType> OnWeaponChange = new UnityEvent<CombatModel.WeaponType>();
    public UnityEvent OnAttack = new UnityEvent();

    public GameObject fist; // Reference to the fist GameObject

    private CombatModel combatModel;

    void Start()
    {
        combatModel = new CombatModel();
        InitializeEvents();

        if (fist != null)
        {
            // Ensure the fist has a collider and set it as a trigger
            Collider fistCollider = fist.GetComponent<Collider>();
            if (fistCollider == null)
            {
                fistCollider = fist.AddComponent<SphereCollider>(); // Example collider
            }
            fistCollider.isTrigger = true;

        }
    }

    private void InitializeEvents()
    {
        OnWeaponChange.AddListener(HandleWeaponChange);
        OnAttack.AddListener(PerformAttack);
    }

    public void HandleWeaponChange(CombatModel.WeaponType weapon)
    {
        combatModel.SetWeapon(weapon);
    }

    public void PerformAttack()
    {
        CombatModel.WeaponType currentWeapon = combatModel.GetCurrentWeapon();
        Debug.Log("Performing attack with weapon: " + currentWeapon);

        // Activate the fist collider if the weapon is Fist
        if (currentWeapon == CombatModel.WeaponType.Fist && fist != null)
        {
            fist.SetActive(true); // Enable the fist collider
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit!");

            // Apply physics effects to the enemy
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            ApplyForce(enemyRb);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Disable the fist collider after the attack
        if (other.CompareTag("Enemy") && fist != null)
        {
            //fist.SetActive(false);
        }
    }

    public CombatModel.WeaponType GetCurrentWeapon()
    {
        return combatModel.GetCurrentWeapon();
    }

    public void ApplyForce(Rigidbody enemyRb){
        if (enemyRb != null)
            {
                Vector3 forceDirection = enemyRb.transform.position - fist.transform.position;
                enemyRb.AddForce(forceDirection.normalized * 500f); // Example force
            }

    }
}
