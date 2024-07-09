using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public int damage = 10; // Damage dealt by the projectile
    public float lifetime = 5f; // Lifetime of the projectile before it is destroyed
    public float homingSensitivity = 0.1f; // Sensitivity of the homing effect

    private Transform target;
    private Vector3 initialDirection;
    private Vector3 correctedTargetPosition;
    
    public GameObject origin; // Store the corrected target position

    public float correction = 0.0f;

    void Start()
    {
        // Destroy the projectile after its lifetime has passed
        Destroy(gameObject, lifetime);

        // Set the initial direction towards the target
        if (target != null)
        {
            initialDirection = (correctedTargetPosition - transform.position).normalized;
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
            // Direction towards the corrected target position
            Vector3 targetDirection = (correctedTargetPosition - transform.position).normalized;

            // Interpolate between the current direction and the target direction
            initialDirection = Vector3.Lerp(initialDirection, targetDirection, homingSensitivity * Time.deltaTime).normalized;

            // Draw a ray towards the corrected target direction for debugging
            Ray ray = new Ray(transform.position, targetDirection);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        }

        // Move the projectile in the new direction
        transform.position += initialDirection * speed * Time.deltaTime;

        // Update the forward direction to the new direction
        transform.forward = initialDirection;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        // Adjust the target position by adding the correction value to the y component
        correctedTargetPosition = target.position;
        correctedTargetPosition.y += correction;
    }

    void OnTriggerEnter(Collider other)
    {
        /// make it reflect off 
        
        // Check if the projectile hits the player
        if (other.CompareTag("Player"))
        {
            // Apply damage to the player
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null && other.GetComponent<InputControllerDiab>().isBlocking == false)
            {
                playerHealth.TakeDamage(damage);
                other.GetComponent<PlayerController>().KnockBackForce(transform.forward * 3);
            }
            else
            {
                other.GetComponent<PlayerController>().KnockBackForce(transform.forward * 5);
            }

            // Destroy the projectile upon collision
            Destroy(gameObject);
        }else if(other.gameObject != origin) {
            // reflect off everything else .... einfalss winkel = ausfalss winkel
            Vector3 reflect = Vector3.Reflect(initialDirection, other.transform.forward);
            initialDirection = reflect;

        }
    }
}
