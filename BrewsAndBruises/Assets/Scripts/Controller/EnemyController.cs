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
    private Animator animator;
    private float timer;
    private Vector3 wanderPoint;
    private AnimationController animationController;
    private Health health;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animationController = GetComponent<AnimationController>();
        health = GetComponent<Health>();
        animationController.RegisterAnimation("Walking", false);
        animationController.RegisterAnimation("Attack", true);
        animationController.RegisterAnimation("BeeingHit", true);

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

    void SetNewWanderPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
        wanderPoint = navHit.position;
    }

    void Wander()
{
    timer += Time.deltaTime;

    if (timer >= wanderTimer)
    {
        SetNewWanderPoint();
        timer = 0;
    }

    // Check if the agent is enabled and on the NavMesh before setting a new destination
    if (agent.enabled && agent.isOnNavMesh)
    {
        agent.SetDestination(wanderPoint);
    }

    // If the agent reaches the wander point (or nearly so), reset the timer
    if (Vector3.Distance(transform.position, wanderPoint) <= 1f)
    {
        timer = wanderTimer;
    }
}


    void ChasePlayer()

{
    
    if (agent.enabled && agent.isOnNavMesh)
    {
        agent.speed = chaseSpeed;
        agent.stoppingDistance = 2.0f;  
        agent.SetDestination(player.position);
    }
}


    public void ApplyPushback(Vector3 force)
    {
        agent.enabled = false;
        rb.isKinematic = false;
        animationController.TriggerAnimation("BeeingHit");
        rb.AddForce(force, ForceMode.Impulse);
        //animator.SetTrigger("Knockdown");
        StartCoroutine(StandUpRoutine());
    }

    private IEnumerator StandUpRoutine()
    {
        yield return new WaitForSeconds(1.5f); // Delay while knocked down
        //animator.SetTrigger("StandUp");
        yield return new WaitForSeconds(1.0f); // Delay for stand up animation
        yield return new WaitForSeconds(0.5f); // Additional delay before reactivating NavMeshAgent
        ReenableNavMeshAgent();
    }

    private void ReenableNavMeshAgent()
{
    // Find a valid NavMesh position close to the current position and only then enable the agent
    NavMeshHit hit;
    if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
    {
        rb.isKinematic = true; // Switch Rigidbody back to kinematic
        transform.position = hit.position; // Relocate to a valid NavMesh position
        agent.enabled = true; // Re-enable the agent
    }
    else
    {
        Debug.LogError("Failed to find a valid NavMesh position near " + transform.position);
    }
}


    private Vector3 FindClosestNavMeshPosition()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            Debug.LogError("No valid NavMesh position found near " + transform.position);
            return Vector3.zero;
        }
    }

    private void AttackPlayer()
    {
        //animator.SetTrigger("Attack");
        animationController.TriggerAnimation("Attack");
    }
}
