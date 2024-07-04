using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerDestroyer : MonoBehaviour
{
    private void Awake()
    {
        // Sicherstellen, dass dieses Objekt nicht zerstört wird, wenn die Szene wechselt
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        // SceneManager.sceneLoaded abonnieren, um auf Szenenwechsel zu reagieren
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Abbestellen, um Speicherlecks zu vermeiden
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // AudioManager-Objekt finden und zerstören
        GameObject audioManager = GameObject.FindWithTag("AudioManager");
        if (audioManager != null)
        {
            Destroy(audioManager);
        }
}
}
