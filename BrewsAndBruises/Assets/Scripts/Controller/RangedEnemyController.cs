using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class RangedEnemyController : MonoBehaviour
{
    public float lookRadius = 10f; // Radius within which the enemy can see the player
    public float chaseSpeed = 5f; // Speed at which the enemy chases the player
    public float wanderRadius = 20f; // Radius within which the enemy will wander
    public float wanderTimer = 5f; // Time to wait at each wander point

    public GameObject projectilePrefab; // Projectile to be shot at the player
    public Transform projectileSpawnPoint; // Spawn point of the projectile
    public float shootCooldown = 3.0f; // Minimum time between shots
    private float lastShootTime = -3.0f; // Initialize to enable immediate shooting

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private AnimationController animationController;
    private Health health;

    public float attackRange = 15.0f; // Range within which the enemy can shoot
    private Vector3 wanderPoint; // Point to wander to
    private float timer; // Timer for wandering

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
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
        if (animationController.IsInAnimationState("Attack") && animationController.IsAnimationFinished())
        {
            animationController.TriggerAnimation("Walking");
        }
        if (animationController.IsAnimationFinished())
        {
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
            agent.stoppingDistance = attackRange;
            agent.SetDestination(player.position);

            // When within attack range, start the shooting sequence
            if (Vector3.Distance(transform.position, player.position) <= attackRange && Time.time > lastShootTime + shootCooldown)
            {
                FacePlayer(); // Face the player before shooting
                StartCoroutine(ShootAtPlayer());
                lastShootTime = Time.time;
            }
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private IEnumerator ShootAtPlayer()
    {
        agent.isStopped = true; // Stop the agent from moving
        animationController.TriggerAnimation("Attack"); // Play attack animation
        yield return new WaitForSeconds(1); // Wait for 1 second before shooting

        // Instantiate and shoot the projectile
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetTarget(player); // Assuming the projectile has a method to set its target

        agent.isStopped = false; // Allow the agent to resume moving
    }

    public void SetTarget(Transform newTarget)
    {
        player = newTarget;
    }

    public void ApplyPushback(Vector3 force)
    {
        agent.enabled = false;
        rb.isKinematic = false;
        Debug.Log("Applying pushback force: " + force);
        rb.AddForce(force, ForceMode.Impulse);
        StartCoroutine(RecoverFromPushback());
    }

    private IEnumerator RecoverFromPushback()
    {
        yield return new WaitForSeconds(1.5f); // Wait for the effects of the pushback to dissipate

        // Optionally add logic to recover posture or re-engage in combat
        animationController.TriggerAnimation("BeingHit"); // Assuming you have a recovery or being hit animation

        yield return new WaitForSeconds(0.5f); // Additional recovery time

        ReenableNavMeshAgent();
    }

    private void ReenableNavMeshAgent()
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
            Debug.LogError("Failed to find a valid NavMesh position near " + transform.position);
            // Optionally include logic to handle cases where no valid NavMesh position is found
        }
    }
}
