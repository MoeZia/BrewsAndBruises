using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player's transform
    public Vector3 offset = new Vector3(0f, 5f, -10f); 
    // safe rotation

    public float smoothSpeed = 0.125f; // Adjust for smoother or more rigid following

    void LateUpdate()
    {
    
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Keep the camera looking at the player
        transform.LookAt(target);
    }
}
