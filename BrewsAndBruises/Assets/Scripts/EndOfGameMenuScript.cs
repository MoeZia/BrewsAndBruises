using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndOfGameMenuScript : MonoBehaviour
{ 
    public GameObject gameOverMenu;
    
    void Start()
    {
        Time.timeScale = 1f;
        gameOverMenu.SetActive(false);
    }


    public void ActivateMenu() {
       gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame() {
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu() {
        SceneManager.LoadScene("GameMenu");
        
    }
}
