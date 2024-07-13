using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;

public class ChargingEnemyController : MonoBehaviour
{
    public float lookRadius = 10f; // Radius within which the enemy can see the player
    public float chaseSpeed = 5f; // Speed at which the enemy chases the player
    public float chargeSpeed = 15f; // Speed during the charge
    public float stopDistance = 10f; // Distance to stop before charging
    public float waitTime = 2f; // Time to wait before charging
    public int chargeDamage = 20; // Damage dealt during charge

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private AnimationController animationController;
    private Health health;

    private bool isCharging = false;

    private bool CorRunning = false;
    private bool isWaiting = false;

    private AudioManager audioManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        audioManager = FindObjectOfType<AudioManager>();
        animationController = GetComponent<AnimationController>();
        health = GetComponent<Health>();

        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true; // Start with Rigidbody in kinematic mode if using NavMesh

        animationController.RegisterAnimation("Walk", false);
        animationController.RegisterAnimation("Charge", true);
        animationController.RegisterAnimation("BeingHit", true);
    }

    void Update()
    { 
        while (!CorRunning)
        {
            
    
        float distance = Vector3.Distance(player.position, transform.position);

        if (isCharging)
        {
            ChargeAtPlayer();
        }
        else if (distance <= lookRadius)
        {
            if (distance > stopDistance)
            {
                ChasePlayer();
            }
            else
            {
                if (!isWaiting)
                {
                    StartCoroutine(WaitBeforeCharge());
                }
            }
        }
        else
        {
            Wander();
        }
    }}

    void ChasePlayer()
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            animationController.TriggerAnimation("Walk");
        }
    }

    IEnumerator WaitBeforeCharge()
    {
        isWaiting = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        isCharging = true;
        agent.isStopped = false;
        animationController.TriggerAnimation("Charge");
    }

    void ChargeAtPlayer()
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.speed = chargeSpeed;
            agent.SetDestination(player.position);
        }
    }

    void Wander()
    {
        animationController.TriggerAnimation("Walk");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isCharging)
        {
            if (collision.gameObject.CompareTag("Player"))
            // 
            {
                //check if player is in front of the enemy
                Vector3 direction = collision.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle > 90)
                {
                    return;
                }else{
                Health playerHealth = collision.gameObject.GetComponent<Health>();
                if (playerHealth != null && collision.gameObject.GetComponent<InputControllerDiab>().isBlocking == false)
                {
                    playerHealth.TakeDamage(chargeDamage);
                    collision.gameObject.GetComponent<PlayerController>().KnockBackForce(transform.forward * 222);
                }
                else
                {
                    collision.gameObject.GetComponent<PlayerController>().KnockBackForce(transform.forward * 222);
                }

                isCharging = false; // Stop charging after hitting the player
                animationController.TriggerAnimation("Walk"); // Resume walking animation
            }
            }
            else
            {
                // Reflect off other objects
                Vector3 reflect = Vector3.Reflect(agent.velocity.normalized, collision.contacts[0].normal);
                agent.velocity = reflect * chargeSpeed;
            }
        }
    }

    public void ApplyPushback(Vector3 force)
    {
        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);
        Coroutine a = StartCoroutine(RecoverFromPushback());
        

    }

    private IEnumerator RecoverFromPushback()
    {
        CorRunning = true;
        yield return new WaitForSeconds(0.5f); // Wait for the effects of the pushback to dissipate

        animationController.TriggerAnimation("BeingHit"); // Trigger being hit animation

        yield return new WaitForSeconds(1.5f); // Additional recovery time
        CorRunning = false;

        ReenableNavMeshAgent();
    }

    private void ReenableNavMeshAgent()
    {
        try
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                transform.position = hit.position; // Ensure the enemy is back on the NavMesh
                agent.enabled = true; // Reactivate the NavMeshAgent
                rb.isKinematic = true; // Return Rigidbody to kinematic mode for NavMeshAgent control
            }
            else
            {
                Debug.LogWarning("Failed to find a valid NavMesh position near " + transform.position);
                HandleInvalidNavMeshPosition();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("An error occurred while trying to re-enable the NavMeshAgent: " + ex.Message);
        }
    }

    private void HandleInvalidNavMeshPosition()
    {
        agent.enabled = false;
        //transform.position = GetSafePosition();
    }

    private Vector3 GetSafePosition()
    {
        return new Vector3(1, 1, 1); // Example safe position
    }
}
