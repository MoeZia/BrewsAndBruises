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

        // Wenn die aktuelle Szene "Lose" ist, starte die Coroutine
        if (currentScene.name == "Lose")
        {
            StartCoroutine(PlayMenuWithDelay(5f)); // 5 Sekunden Verzögerung
        }
        else
        {
            // In anderen Szenen sofort abspielen
            FindObjectOfType<AudioManager>().Play("menu");
        }
    }

    IEnumerator PlayMenuWithDelay(float delay)
    {
        // Warte die angegebene Zeit
        yield return new WaitForSeconds(delay);

        // Spiele den Sound ab
        FindObjectOfType<AudioManager>().Play("menu");
    }

    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("click");
        FindObjectOfType<AudioManager>().Stop("menu");
        FindObjectOfType<AudioManager>().Play("BackgroundMusic");
        FindObjectOfType<AudioManager>().Play("BackgroundPeople");

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
