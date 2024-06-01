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
        if(string.IsNullOrEmpty(triggerTag) && !collider.gameObject.CompareTag(triggerTag)) return;
        GameObject eventSystem = GameObject.Find("EventSystem");
        Debug.Log( eventSystem.GetComponent<EndOfGameMenuScript>());
        eventSystem.GetComponent<EndOfGameMenuScript>().ActivateMenu();
       
    }


}
