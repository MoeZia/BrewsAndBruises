using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndOfGameMenuScript : MonoBehaviour
{ 
    public GameObject gameOverMenu;
    public GameObject tutorialFrame;
    public GameObject tutorialButton;
    public GameObject hud;
    
    void Start()
    {
        Time.timeScale = 1f;
        gameOverMenu.SetActive(false);
    }


    public void ActivateMenu() {

       gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ActivateTutorial()
    {
        tutorialFrame.SetActive(true);
        tutorialButton.SetActive(false);
        hud.SetActive(false);
    }
        public void DectivateTutorial()
    {
        tutorialFrame.SetActive(false);
        tutorialButton.SetActive(true);
        hud.SetActive(true);
    }

    public void RestartGame() {
        FindObjectOfType<AudioManager>().Play("click");
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu() {
        FindObjectOfType<AudioManager>().Play("click");
        SceneManager.LoadScene("GameMenu");
        
    }

    public void setHeadLine(string text) {
        gameOverMenu.GetComponentInChildren<TMP_Text>().text = text;
    }
}
