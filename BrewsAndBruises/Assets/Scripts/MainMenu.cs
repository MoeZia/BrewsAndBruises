using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        // Überprüfe die aktuelle Szene
        Scene currentScene = SceneManager.GetActiveScene();

        FindObjectOfType<AudioManager>().Stop("intro");
        // Wenn die aktuelle Szene "Lose" ist, starte die Coroutine
        if (currentScene.name == "Lose")
        {
            StartCoroutine(PlayMenuWithDelay(5f, "menu")); // 5 Sekunden Verzögerung
        }
        else if (currentScene.name == "Win")
        {
            FindObjectOfType<AudioManager>().Play("applaus");
            StartCoroutine(PlayMenuWithDelay(7f, "menu"));
        }
        else
        {
            // In anderen Szenen sofort abspielen
            FindObjectOfType<AudioManager>().Play("menu");
        }
    }

    IEnumerator PlayMenuWithDelay(float delay, string songtitle)
    {
        // Warte die angegebene Zeit
        yield return new WaitForSeconds(delay);

        // Spiele den Sound ab
        FindObjectOfType<AudioManager>().Play(songtitle);
    }

    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("click");
        FindObjectOfType<AudioManager>().Stop("menu");
        FindObjectOfType<AudioManager>().Play("BackgroundPeople");
        FindObjectOfType<AudioManager>().Play("intro");
        StartCoroutine(PlayMenuWithDelay(50f, "BackgroundMusic"));
        
        SceneManager.LoadSceneAsync("Game");
    }

    public void Tutorial()
    {
        FindObjectOfType<AudioManager>().Play("click");
        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("click");
        Application.Quit();
    }

}
