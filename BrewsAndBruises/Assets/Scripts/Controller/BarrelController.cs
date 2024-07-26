using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class BarrelController : MonoBehaviour
{
    private Health health;
    private Vector3 originalScale;

    void Start()
    {
        health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("Health component not found on the barrel.");
            return;
        }
        
        originalScale = transform.localScale;  // Store the original scale
        health.OnHealthChanged.AddListener(OnHealthChanged);  // Listen to health changes
    }

    void OnDestroy()
    {
        if (health != null)
        {
            health.OnHealthChanged.RemoveListener(OnHealthChanged);  // Clean up listener on destroy
        }
        
        //SceneManager.LoadSceneAsync("Win", );
    }

    private void OnHealthChanged(string id, int currentHealth, int maxHealth)
    {
        if (currentHealth < maxHealth)
        {
            ShrinkAndRestore();
        }
    }

    private void ShrinkAndRestore()
    {
        StartCoroutine(ShrinkAndRestoreCoroutine());
    }

    private IEnumerator ShrinkAndRestoreCoroutine()
    {
        transform.localScale *= 0.99f;  // Shrink by 1%
        yield return new WaitForSeconds(0.5f);  // Wait for 0.5 seconds
        transform.localScale = originalScale;  // Restore original scale
    }
    
    private IEnumerator LoadWinScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync("Win");
    }
}
