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
    public bool itemsCollectedBoxShown = false;

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
        dialouge.Add("Hello 'Unknown Person'.             ");
        dialouge.Add("Welcome to the moon mission saftey training course.        ");
        dialouge.Add("In this senario you have crashed on a unknown planet.       ");
        dialouge.Add("To help with the realism of this test we have given you amnesia.       ");
        dialouge.Add("Now you need to try and get off this planet by gathering supplies.      ");
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
            dialouge.Add("Good job! You've passed the first test.          ");
            dialouge.Add("Now try using the left mouse button on one of those trees.          ");
            walkBoxShown = true;
        }

        if (playerScript.objectsHit > 3 && punchTreeBoxShown == false) {
            dialouge.Add("Well done! You now have a 0.01% chance of survival.         ");
            dialouge.Add("Try gathering some more supplies and see if you can find anything intresting.         ");
            punchTreeBoxShown = true;
        }

        if (playerScript.itemsPickedUp > 50 && itemsCollectedBoxShown == false) {
            dialouge.Add("That should be enough items. See if there is anything important lying around here         ");
            playerScript.SpawnKey();
            itemsCollectedBoxShown = true;
        }
    }
}
