using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject loadingScreen;
    public void startGame() {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("SampleScene");
    }

    public void quit() {
        // Scared?
        Application.Quit();
    }
}
