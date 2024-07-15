using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetWeapon : MonoBehaviour
{
    public float attackDuration = 2.0f;
    public float staminaCostPerSecond = 59f;
    public float pushBackForce = 10f;
    public int damage = 1;
    public float attackRange = 5f;

    private bool isAttacking = false;
    private StaminaHUD staminaHUD;
    private AudioManager audioManager;

    private AnimationController animationController;

    

    void Start()
    {
        staminaHUD = FindObjectOfType<StaminaHUD>(); // Find the StaminaHUD in the scene
        audioManager = FindObjectOfType<AudioManager>();
        animationController = GetComponentInParent<AnimationController>();
        animationController.RegisterAnimation("Trumpet", true);
    }

    void Update()
    {/*
        if (Input.GetButton("Fire1") && !isAttacking && staminaHUD.UseStamina(staminaCostPerSecond * Time.deltaTime))
        {
            //StartAttack();
        }
        else if (!Input.GetButton("Fire1"))
        {
            StopAttack();
        }
        //*/

        if (isAttacking)

        {
            if(staminaHUD.currentStamina>10f){
                staminaHUD.UseStamina(staminaCostPerSecond * Time.deltaTime);
            }
            if ( staminaHUD.currentStamina < 10f)
            {
                StopAttack();
            }
            ApplyDamageAndPushBack();
        }
    }

    public void StartAttack()
    {
        if (!isAttacking&& staminaHUD.currentStamina > 90f)

        {
            Debug.Log("Stamina : "+ staminaHUD.currentStamina);
            isAttacking = true;
            // Add visual/audio effects for starting the attack
            audioManager.Play("trumpet");
            //sad
            animationController.TriggerAnimation("Trumpet"); 
        }
    }

    public void StopAttack()
    {
        if (isAttacking)
        {
            isAttacking = false;
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
