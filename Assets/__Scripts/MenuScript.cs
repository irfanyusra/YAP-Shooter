using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    //sets up the audio sources
    public AudioSource menuMusicAS;
    public AudioClip menuMusicAC;

    private void Start()
    {
        //links audio sources and clips
        menuMusicAS.clip = menuMusicAC;
        menuMusicAS.Play();
    }

    public void playGame() // loads the next game using scene manager and project settings
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() // quits game
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

}
