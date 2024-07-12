using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f; // Radius within which the enemy can see the player
    public float chaseSpeed = 5f; // Speed at which the enemy chases the player
    public float wanderRadius = 20f; // Radius within which the enemy will wander
    public float wanderTimer = 5f; // Time to wait at each wander point

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private AnimationController animationController;
    private Health health;

    public float attackRange = 2.0f; // Range within which the enemy can attack
    private float attackCooldown = 4.0f; // Minimum time between attacks
    private float lastAttackTime = -4.0f; // Initialize to enable immediate attack

    private Vector3 wanderPoint; // Point to wander to
    private float timer; // Timer for wandering

    public int damage = 10; // Damage value for the enemy attack

    private AudioManager audioManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        audioManager = FindObjectOfType<AudioManager>();
        animationController = GetComponent<AnimationController>();
        health = GetComponent<Health>();
        animationController.RegisterAnimation("Walking", false);
        animationController.RegisterAnimation("Attack", true);
        animationController.RegisterAnimation("BeingHit", true);

        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true; // Start with Rigidbody in kinematic mode if using NavMesh
        timer = wanderTimer;
        SetNewWanderPoint();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if(animationController.IsInAnimationState("Attack") && animationController.IsAnimationFinished())
    {
       //Debug.Log("Walking");
       animationController.TriggerAnimation("Walking");
    }
    if( animationController.IsAnimationFinished())
    {
       //Debug.Log("Walking");
       animationController.TriggerAnimation("Walking");
    }

        if (distance <= lookRadius)
        {
            ChasePlayer();
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            SetNewWanderPoint();
            timer = 0;
        }

        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(wanderPoint);
        }

        if (Vector3.Distance(transform.position, wanderPoint) <= 1f)
        {
            timer = wanderTimer; // Reset the timer when the destination is reached
        }
    }

    void SetNewWanderPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
        wanderPoint = navHit.position;
    }

    void ChasePlayer()
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.speed = chaseSpeed;
            agent.stoppingDistance = 2.0f;
            agent.SetDestination(player.position);

            // When close to the player, start the attack sequence
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                StartCoroutine(AttackSequence());
            }
        }
    }

    private IEnumerator AttackSequence()
    {
        agent.isStopped = true; // Stop the agent from moving
        animationController.TriggerAnimation("Attack"); // adding attack so he can also miss the player and not just hit him all the time
        yield return new WaitForSeconds(2); // Wait for 2 seconds before checking and attacking

        // Check for the player within attack range using Physics.OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player") && Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer(hitCollider); // Perform the attack
                lastAttackTime = Time.time; // Reset the attack timer
                break; // Assuming only one player, break after attacking
            }
        }

        agent.isStopped = false; // Allow the agent to resume moving
    }

    private void AttackPlayer(Collider playerCollider)
    {
        audioManager.Play("enemy_slash");
        //animationController.TriggerAnimation("Attack");
        PlayerController PlayerController = playerCollider.GetComponent<PlayerController>();// Example knockback direction
        if(playerCollider.GetComponent<InputControllerDiab>().isBlocking==false){
            
            playerCollider.GetComponent<Health>().TakeDamage(damage);
            PlayerController.KnockBackForce(transform.forward * 4);

        }else{
            PlayerController.KnockBackForce(transform.forward * 8);
        }
        
    }

   public void ApplyPushback(Vector3 force)
    {
        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);
        StartCoroutine(RecoverFromPushback());
    }

    private IEnumerator RecoverFromPushback()
    {
        yield return new WaitForSeconds(0.5f); // Wait for the effects of the pushback to dissipate

        // Optionally add logic to recover posture or re-engage in combat
        animationController.TriggerAnimation("BeingHit"); // Assuming you have a recovery or being hit animation

        yield return new WaitForSeconds(1.5f); // Additional recovery time

        ReenableNavMeshAgent();
    }

    private void ReenableNavMeshAgent()
{
    try
    {
        // Check if the position is still on the NavMesh
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
            // Optionally include logic to handle cases where no valid NavMesh position is found
            HandleInvalidNavMeshPosition();
        }
    }
    catch (System.Exception ex)
    {
        Debug.LogError("An error occurred while trying to re-enable the NavMeshAgent: " + ex.Message);
        // Optionally handle the exception in a specific way
    }
}

private void HandleInvalidNavMeshPosition()
{
    // Implement logic to handle the situation where no valid NavMesh position is found
    // For example, you could disable the agent, move to a default position, or take other actions
    agent.enabled = false;
    // Move to a safe position or take other appropriate action
    transform.position = GetSafePosition();
}

private Vector3 GetSafePosition()
{
    // Return a default or safe position on the NavMesh
    // This is just an example, you should implement it according to your game's needs
    return new Vector3(1, 1, 1); // Example safe position
}

}




