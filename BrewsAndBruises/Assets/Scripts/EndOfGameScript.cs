using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGameScript : MonoBehaviour
{
    public string triggerTag = "Player";

    Camera mainCamera;

    void Start() {
        mainCamera = Camera.main;
    }
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag(triggerTag)) {
            GameObject eventSystem = GameObject.Find("EventSystem");
            eventSystem.GetComponent<EndOfGameMenuScript>().ActivateMenu();
        }
    }
}
