using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f; // Radius within which the enemy can see the player
    public float chaseSpeed = 5f; // Speed at which the enemy chases the player
    public float wanderRadius = 20f; // Radius within which the enemy will wander
    public float wanderTimer = 5f; // Time to wait at each wander point

    private Transform player;
    private NavMeshAgent agent;
    private float timer;
    private Vector3 wanderPoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        SetNewWanderPoint();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

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

        agent.SetDestination(wanderPoint);

        if (Vector3.Distance(transform.position, wanderPoint) <= 1f)
        {
            timer = wanderTimer;
        }
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
