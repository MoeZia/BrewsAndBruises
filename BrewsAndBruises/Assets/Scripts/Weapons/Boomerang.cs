using UnityEngine;
using System.Collections;

public class Boomerang : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float speed = 10f; // Speed of the boomerang
    public float rotationSpeed = 360f; // Rotation speed in degrees per second around its local Y-axis

    private Vector3 initialRelativePosition; // Initial position relative to the player
    private Quaternion initialRelativeRotation; // Initial rotation relative to the player
    private bool isReturning = false; // Flag to control the return phase

    void Start()
    {
        // Store the initial relative position and rotation to the player at the start
        initialRelativePosition = transform.localPosition;
        initialRelativeRotation = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isReturning) // Left mouse button to throw
        {
            StartCoroutine(ThrowBoomerang());
        }

        if (isReturning)
        {
            UpdateReturnPosition();
        }
    }

    IEnumerator ThrowBoomerang()
    {
        // Calculate the target point based on the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 targetPoint = hit.point;
            targetPoint.y = playerTransform.position.y + initialRelativePosition.y; // Maintain the initial relative height

            // Move towards the target point
            yield return FlyToTarget(targetPoint);

            // Start returning after reaching the target
            isReturning = true;
        }
    }

    void UpdateReturnPosition()
    {
        Vector3 returnPosition = playerTransform.position + initialRelativePosition;
        returnPosition.y = playerTransform.position.y + initialRelativePosition.y; // Maintain the initial relative height

        transform.position = Vector3.MoveTowards(transform.position, returnPosition, speed * Time.deltaTime);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, initialRelativeRotation, rotationSpeed * Time.deltaTime);

        // Stop the return if close enough to the return position
        if (Vector3.Distance(transform.position, returnPosition) < 0.1f)
        {
            isReturning = false;
            transform.localPosition = initialRelativePosition; // Snap to the initial relative position
            transform.localRotation = initialRelativeRotation; // Snap to the initial relative rotation
        }
    }

    IEnumerator FlyToTarget(Vector3 target)
    {
        Vector3 startPosition = transform.position;
        float journeyLength = Vector3.Distance(startPosition, target);
        float journeyDuration = journeyLength / speed;
        float elapsedTime = 0f;

        while (elapsedTime < journeyDuration)
        {
            float t = elapsedTime / journeyDuration;
            transform.position = Vector3.Lerp(startPosition, target, t);
           // transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);  // Rotate around local Y-axis
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = target;  // Ensure the boomerang reaches the target position
    }
}
