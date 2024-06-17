using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public int damage = 10; // Damage dealt by the projectile
    public float lifetime = 5f; // Lifetime of the projectile before it is destroyed
    public float homingSensitivity = 0.1f; // Sensitivity of the homing effect

    private Transform target;
    private Vector3 initialDirection;

    void Start()
    {
        // Destroy the projectile after its lifetime has passed
        Destroy(gameObject, lifetime);

        // Set the initial direction towards the target
        if (target != null)
        {
            initialDirection = (target.position - transform.position).normalized;
        }
        else
        {
            initialDirection = transform.forward;
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Direction towards the target
            Vector3 targetDirection = (target.position - transform.position).normalized;

            // Interpolate between the current direction and the target direction
            initialDirection = Vector3.Lerp(initialDirection, targetDirection, homingSensitivity * Time.deltaTime).normalized;
        }

        // Move the projectile in the new direction
        transform.position += initialDirection * speed * Time.deltaTime;

        // Update the forward direction to the new direction
        transform.forward = initialDirection;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the projectile hits the player
        if (other.CompareTag("Player"))
        {
            // Apply damage to the player
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                other.GetComponent<PlayerController>().KnockBackForce(transform.forward * 2);
            }

            // Destroy the projectile upon collision
            Destroy(gameObject);
        }

        
        
    }
}
