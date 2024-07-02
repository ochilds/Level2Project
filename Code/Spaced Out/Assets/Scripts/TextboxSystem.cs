using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextboxSystem : MonoBehaviour
{
    public GameObject RobotIcon;
    public GameObject RobotTextBox;
    public List<string> dialouge = new List<string>();
    public int state = 0;
    public GameObject player;
    public PlayerController playerScript;
    public bool walkBoxShown = false;
    public bool punchTreeBoxShown = false;

    public void ShowRobotTextbox(string textToShow) {
        state = 1;
        // Delete the current text and show icon+textbox
        RobotTextBox.GetComponent<TextMeshProUGUI>().text = "";
        RobotIcon.SetActive(true);
        RobotTextBox.SetActive(true);
        // Start the text box typing the string
        RobotTextBox.GetComponent<TypingSystem>().ToType = textToShow;
    }

    public void HideRobotTextbox() {
        state = 0;
        // Delete current text and hide icon+textbox
        RobotTextBox.GetComponent<TextMeshProUGUI>().text = "";
        RobotIcon.SetActive(false);
        RobotTextBox.SetActive(false);
    }

    private void Start() {
        // Initilize player script
        playerScript = player.GetComponent<PlayerController>();
        // Add initial dialouge
        dialouge.Add("Oh... You're alive.... Initiliazing panic control system........          ");
        dialouge.Add("Thank you for purchasing this spaceship from Nebula Makers Inc.          ");
        dialouge.Add("'Where saftey is always first'          ");
        dialouge.Add("Unfortunatly we forgot to put the hyperspeed engine in your ship and so, you crashed.        ");
        dialouge.Add("Fortunatly you have landed on a planet that from short scans shows all the nessercary materials to make an engine.        ");
        dialouge.Add("If you could gather those materials you could make and install your own engine.       ");
        dialouge.Add("First, try moving with the WASD keys and look around with the mouse.          ");
    }

    private void FixedUpdate() {
        // If there is a dialouge box to show
        if (dialouge.Count > 0) {
            // If state equals 0
            if (state == 0) {
                // Show the textbox and remove dialouge from list
                ShowRobotTextbox(dialouge[0]);
                dialouge.RemoveAt(0);
            }
        }
        // If there is no dialouge to show and state equals 0 hide textbox
        if (dialouge.Count == 0 && state == 0) {
            HideRobotTextbox();
        }

        if (playerScript.distanceTraveled > 100 && playerScript.distanceTraveled > 100 && walkBoxShown == false) {
            dialouge.Add("Good job! I'll note down that you're not a massive idiot.          ");
            dialouge.Add("Now try using the left mouse button on one of those trees.          ");
            walkBoxShown = true;
        }

        if (playerScript.objectsHit > 3 && punchTreeBoxShown == false) {
            dialouge.Add("Well done! You now have a 0.01% chance of survival.         ");
            dialouge.Add("To make a engine you'll first need to make tools.         ");
            dialouge.Add("Try gathering some more wood and use the crafting table on the ship.         ");
            punchTreeBoxShown = true;
        }
    }
}
