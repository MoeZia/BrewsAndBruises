using UnityEngine;

public class ProjectileGrenade : MonoBehaviour
{
    public float throwSpeed = 15f; // Speed of the grenade when thrown
    public int damage = 50; // Damage dealt by the grenade explosion
    public float explosionRadius = 5f; // Radius of the explosion
    public float blinkDuration = 2f; // Time for which the grenade blinks before exploding
    public GameObject explosionEffect; // Explosion effect prefab

    private bool hasLanded = false;
    private float blinkTimer = 0f;
    private Renderer grenadeRenderer;
    private bool isBlinking = false;

    void Start()
    {
        // Set initial velocity for the throw
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * throwSpeed;
        }

        grenadeRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (hasLanded)
        {
            blinkTimer += Time.deltaTime;

            if (blinkTimer >= blinkDuration)
            {
                Explode();
            }
            else
            {
                // Handle blinking effect
                if ((int)(blinkTimer * 10) % 2 == 0)
                {
                    if (!isBlinking)
                    {
                        grenadeRenderer.enabled = !grenadeRenderer.enabled;
                        isBlinking = true;
                    }
                }
                else
                {
                    isBlinking = false;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Stop the grenade movement when it lands
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        hasLanded = true;
    }

    void Explode()
    {
        // Show explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Deal damage to all objects within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Health health = nearbyObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // You can add additional logic to apply forces, etc.
        }

        // Destroy the grenade object
        Destroy(gameObject);
    }
}
