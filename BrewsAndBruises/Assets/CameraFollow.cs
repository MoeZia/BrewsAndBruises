using UnityEngine;
using System.Collections;


public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player's transform
    public Vector3 offset = new Vector3(-19.5f, 12.5f, 4.5f); // Offset for the camera position
    public float smoothSpeed = 0.125f; // Adjust for smoother or more rigid following
    public Transform[] pathPoints; // List of GameObjects representing the path
    public float pathTraversalTime = 50f; // Time to traverse the path in seconds

    private int currentPathIndex = 0; // Index of the current path point
    private bool isFollowingPath = true; // Flag to check if the camera is following the path
    float totalTime = 0f;

    public GameObject continueButton;
    private bool continuePressed = false;

    void Start()
    {
        if (pathPoints.Length > 0)
        {
            StartCoroutine(FollowPath());
            gameObject.GetComponentInChildren<Canvas>().enabled = false;
        }
    }

    void LateUpdate()
    {
        if (!isFollowingPath)
        {
            FollowTarget();
            gameObject.GetComponentInChildren<Canvas>().enabled = true;
        }
    }

    IEnumerator FollowPath()
    {
        totalTime = 0f;
        while (currentPathIndex < pathPoints.Length && isFollowingPath)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = pathPoints[currentPathIndex].position + offset;
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.LookRotation(pathPoints[currentPathIndex].position - transform.position);
            float elapsedTime = 0f;

            while (elapsedTime < pathTraversalTime / pathPoints.Length )
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime * pathPoints.Length) / pathTraversalTime);
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, (elapsedTime * pathPoints.Length) / pathTraversalTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = endPosition;
            transform.rotation = endRotation;
            currentPathIndex++;
        }

        isFollowingPath = false; // Path traversal completed
    }
    void FollowTarget()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Keep the camera looking at the player
        transform.LookAt(target);
    }

    public void SkipPath()
    {
        isFollowingPath = false;
        continueButton.SetActive(false);
    }
}
