using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour
{
    public UnityEvent<CombatModel.WeaponType> OnWeaponChange = new UnityEvent<CombatModel.WeaponType>();
    public UnityEvent OnAttack = new UnityEvent();

    public GameObject fist; // Reference to the fist GameObject

    private CombatModel combatModel;

    private Health enemyHealth;

    bool isAttacking = false;
    float delta = 0;

    int currDamage;

    void Start()
    {
     
        combatModel = new CombatModel();
        InitializeEvents();

        if (fist != null)
        {
            // Ensure the fist has a collider and set it as a trigger
            SphereCollider fistCollider = fist.GetComponent<SphereCollider>();
            if (fistCollider == null)
            {
                // with small radius to avoid collision with other objects
                fistCollider = fist.AddComponent<SphereCollider>();
                fistCollider.radius = 0.05f;
                fistCollider.isTrigger = true;
                
                 // Example collider
            }
            fistCollider.isTrigger = true;

        }
    }

    private void InitializeEvents()
    {
        OnWeaponChange.AddListener(HandleWeaponChange);
        OnAttack.AddListener(PerformAttack);
    }
    void Update(){
        // if isattacking = true deactivate after 1 second#
        //Debug.Log(isAttacking);
    
        delta += Time.deltaTime;
        if (delta > 1)
        {
            isAttacking = false;
            delta = 0;
        }
    }

    public void HandleWeaponChange(CombatModel.WeaponType weapon)
    {
        combatModel.SetWeapon(weapon);
    }

    public void PerformAttack()
    {
        CombatModel.WeaponType currentWeapon = combatModel.GetCurrentWeapon();
        currDamage = combatModel.GetDamage();
        // set bool attacking to true for the animation

        isAttacking = true;
        Debug.Log("In Combat controller--Performing attack with weapon: " + currentWeapon);

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
            enemyHealth = other.GetComponent<Health>();

            //aply force only if input was given -- public UnityEvent OnAttack;
            if(isAttacking)
            {
                ApplyForce(enemyRb);
                enemyHealth.TakeDamage(currDamage);
                Debug.Log("Applying force to enemy"+enemyRb.name+" and Damage "+currDamage);
            }
            //ApplyForce(enemyRb);
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
