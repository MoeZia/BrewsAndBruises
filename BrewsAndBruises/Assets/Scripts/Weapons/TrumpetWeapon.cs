using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetWeapon : MonoBehaviour
{
    public float attackDuration = 3.0f; // Attack lasts for 3 seconds
    public float staminaCostPerSecond = 34f;
    public float pushBackForce = 10f;
    public int damage = 1;
    public float attackRange = 5f;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private StaminaHUD staminaHUD;
    private AudioManager audioManager;

    void Start()
    {
        staminaHUD = FindObjectOfType<StaminaHUD>(); // Find the StaminaHUD in the scene
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking && staminaHUD.currentStamina > 10)
        {
            StartAttack();
        }

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration || !staminaHUD.UseStamina(staminaCostPerSecond * Time.deltaTime))
            {
                StopAttack();
            }

            ApplyDamageAndPushBack();
        }
    }

    public void StartAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            attackTimer = 0f; // Reset the attack timer
            // Add visual/audio effects for starting the attack
            audioManager.Play("trumpet");
        }
    }

    public void StopAttack()
    {
        if (isAttacking)
        {
            isAttacking = false;
            attackTimer = 0f; // Reset the attack timer
            // Add visual/audio effects for stopping the attack
            audioManager.Stop("trumpet");
        }
    }

    void ApplyDamageAndPushBack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Apply damage to the enemy
                Health enemyH = hitCollider.GetComponent<Health>();
                if (enemyH != null)
                {
                    enemyH.TakeDamage(damage);
                }

                EnemyController enemy = hitCollider.GetComponent<EnemyController>();
                RangedEnemyController rangedEnemy = hitCollider.GetComponent<RangedEnemyController>();
                if (enemy != null)
                {
                    Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
                    enemy.ApplyPushback(pushDirection * pushBackForce);
                }

                if (rangedEnemy != null)
                {
                    Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
                    rangedEnemy.ApplyPushback(pushDirection * pushBackForce);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
