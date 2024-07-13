using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public Transform hand; // Reference to the hand Transform
    public float speed = 10f; // Speed of the projectile
    public float returnDelay = 0.5f; // Delay before returning to the hand
    public float projectileHeight = 1f; // Height at which the projectile should move
    public Color rayColor = Color.red; // Color of the debug ray

    private Vector3 targetPosition;
    private bool isReturning = false;
    private bool isFlying = false;

    public float forcefactor = 12f;
    public int damgeAmount = 15;
    public float staminaCost = 24f; // Stamina cost for throwing the boomerang

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private StaminaHUD staminaHUD; // Reference to the StaminaHUD

    private float maxDistance = 10f;

    void Start()
    {
        // Save the initial local position and rotation relative to the hand
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
        staminaHUD = FindObjectOfType<StaminaHUD>(); // Find the StaminaHUD in the scene
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFlying &&IsInRange(GetMouseWorldPosition()))

        {
            if(staminaHUD.UseStamina(staminaCost)){
            // Calculate target position in world space
            targetPosition = GetMouseWorldPosition();
            
            isFlying = true;
            transform.SetParent(null); // Detach from hand to move freely
            }
        }

        if (isFlying)
        {
            if (!isReturning)
            {
                // Look at the target position and move towards it
                Vector3 adjustedTargetPosition = new Vector3(targetPosition.x, projectileHeight, targetPosition.z);
                transform.LookAt(adjustedTargetPosition);
                transform.position = Vector3.MoveTowards(transform.position, adjustedTargetPosition, speed * Time.deltaTime);

                Debug.DrawLine(transform.position, adjustedTargetPosition, rayColor);

                if (Vector3.Distance(transform.position, adjustedTargetPosition) < 0.1f)
                {
                    isReturning = true;
                    Invoke("ReturnToHand", returnDelay);
                }
            }
            else
            {
                // Look at the hand position and move back to it
                Vector3 adjustedHandPosition = new Vector3(hand.position.x, projectileHeight, hand.position.z);
                transform.LookAt(adjustedHandPosition);
                transform.position = Vector3.MoveTowards(transform.position, adjustedHandPosition, speed * Time.deltaTime);

                Debug.DrawLine(transform.position, adjustedHandPosition, rayColor);

                if (Vector3.Distance(transform.position, adjustedHandPosition) < 0.1f)
                {
                    isFlying = false;
                    isReturning = false;
                    transform.SetParent(hand);
                    // Return to the initial local position and rotation
                    transform.localPosition = initialLocalPosition;
                    transform.localRotation = initialLocalRotation;
                }
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Convert mouse position to world position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return new Vector3(hit.point.x, projectileHeight, hit.point.z);
        }
        return new Vector3(transform.position.x, projectileHeight, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFlying && other.CompareTag("Enemy") && other.gameObject.layer == LayerMask.NameToLayer("Hittable"))
        {
            // Handle hit on enemy
            EnemyController enemy = other.GetComponent<EnemyController>();
            RangedEnemyController rangedEnemy = other.GetComponent<RangedEnemyController>();
            if (enemy != null)
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                enemy.ApplyPushback(pushDirection * forcefactor);
            }
            if (rangedEnemy != null)
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                rangedEnemy.ApplyPushback(pushDirection * forcefactor);
            }
            other.GetComponent<Health>().TakeDamage(damgeAmount);
        }
    }

    private void ReturnToHand()
    {
        isReturning = true;
    }
    public bool IsInRange(Vector3 target){
        return Vector3.Distance(target,transform.position) < maxDistance;
    }
}
