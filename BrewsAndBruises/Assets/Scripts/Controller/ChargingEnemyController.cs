using UnityEngine;

public class ChargingEnemyController : MonoBehaviour
{
    public float stopDistance = 10f; // Distance to stop before charging
    public float waitTime = 2f; // Time to wait before charging
    public float chargeSpeed = 15f; // Speed during the charge
    public float normalSpeed = 5f; // Normal movement speed
    public int damage = 20; // Damage dealt during charge

    private Transform player;
    private bool isCharging = false;
    private float waitTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assume player has tag "Player"
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isCharging)
        {
            ChargeAtPlayer();
        }
        else if (distanceToPlayer > stopDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isCharging = true;
                waitTimer = 0f;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * normalSpeed * Time.deltaTime;
    }

    void ChargeAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chargeSpeed * Time.deltaTime;

        // Implement damage logic here, if required
        // For example, check for collisions with other objects and apply damage
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isCharging)
        {
            // Apply damage to player or other objects in the path
            if (collision.gameObject.CompareTag("Player"))
            {
                // Assuming the player has a method to take damage
                collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            }

            // Stop charging after hitting something
            isCharging = false;
        }
    }
}
