using UnityEngine;
using System.Collections;

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
    public void Hitshake()
    {
        // Shake the camera when the player is hit
        StartCoroutine(Shake(0.15f, 1f));
    }
    //shake the camera
    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = offset;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            offset = originalPos + new Vector3(0,0, Random.Range(-1f, 1f) * magnitude);
            elapsed += Time.deltaTime;
           yield return null;
           
        }
        offset = originalPos;
    }
    
}
