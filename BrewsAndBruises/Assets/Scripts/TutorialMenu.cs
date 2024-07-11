using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialMenu : MonoBehaviour
{     
    void Start()
    {
        FindObjectOfType<AudioManager>().Stop("menu");
        FindObjectOfType<AudioManager>().Play("tutorial");
    }

    public void BackToMenu() {
        FindObjectOfType<AudioManager>().Play("click");
        FindObjectOfType<AudioManager>().Stop("tutorial");
        SceneManager.LoadScene("GameMenu");
    }
}
