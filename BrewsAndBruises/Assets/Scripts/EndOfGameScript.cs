using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGameScript : MonoBehaviour
{
    public string triggerTag = "Player";

    Camera mainCamera;

    void Start() {
        mainCamera = Camera.main;
    }
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag(triggerTag)) {
            //GameObject eventSystem = GameObject.Find("EventSystem");
            //eventSystem.GetComponent<EndOfGameMenuScript>().ActivateMenu();
            FindObjectOfType<AudioManager>().Stop("BackgroundMusic");
            FindObjectOfType<AudioManager>().Stop("BackgroundPeople");
            SceneManager.LoadSceneAsync("Win");
        }
    }
}
