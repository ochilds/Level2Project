using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextboxSystem : MonoBehaviour
{
    public GameObject RobotIcon;
    public GameObject RobotTextBox;

    public void ShowRobotTextbox(string textToShow) {
        // Delete the current text and show icon+textbox
        RobotTextBox.GetComponent<Text>().text = "";
        RobotIcon.SetActive(true);
        RobotTextBox.SetActive(true);
        // Start the text box typing the string
        RobotTextBox.GetComponent<TypingSystem>().ToType = textToShow;
    }

    public void HideRobotTextbox() {
        // Delete current text and hide icon+textbox
        RobotTextBox.GetComponent<Text>().text = "";
        RobotIcon.SetActive(false);
        RobotTextBox.SetActive(false);
    }
}
