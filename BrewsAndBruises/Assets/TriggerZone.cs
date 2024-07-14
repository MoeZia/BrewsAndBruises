using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    Collider col;
    public GameObject triggerZone;

    // public variable for audioclip
    public string audioClipName;

    private AudioManager audioManager;

    bool isTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        col = triggerZone.GetComponent<Collider>();
        col.isTrigger = true;

        audioManager = FindObjectOfType<AudioManager>();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && !isTriggered)
        {
            isTriggered = true;

            // Play audio and activate child objects
            audioManager.Stop("intro");
            audioManager.Play(audioClipName);
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
