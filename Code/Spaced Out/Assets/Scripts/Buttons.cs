using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    // This is to control the buttons on the title screen
    public GameObject loadingScreen;
    public void startGame() {
        // Load the game
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("SampleScene");
    }

    public void quit() {
        // Scared?
        Application.Quit();
    }
}
