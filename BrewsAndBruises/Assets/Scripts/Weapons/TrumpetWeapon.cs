using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetWeapon : MonoBehaviour
{
    public float attackDuration = 1.0f;
    public float staminaCostPerSecond = 10f;
    public float pushBackForce = 10f;
    public int damage = 1;
    public float attackRange = 5f;

    private float currentStamina;
    public float maxStamina = 100f;
    private bool isAttacking = false;

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && currentStamina > 0)
        {
            StartAttack();
        }
        else
        {
            StopAttack();
        }

        if (isAttacking)
        {
            currentStamina -= staminaCostPerSecond * Time.deltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                StopAttack();
            }
            ApplyDamageAndPushBack();
        }

        // Regenerate stamina over time or other stamina management
        currentStamina = Mathf.Min(maxStamina, currentStamina + Time.deltaTime * 5); // example regeneration rate
    }

    void StartAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            // Add visual/audio effects for starting the attack
        }
    }

    void StopAttack()
    {
        if (isAttacking)
        {
            isAttacking = false;
            // Add visual/audio effects for stopping the attack
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
            if (enemy != null) {
                Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
                enemy.ApplyPushback(pushDirection * pushBackForce);
            }

            if(rangedEnemy != null) {
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

