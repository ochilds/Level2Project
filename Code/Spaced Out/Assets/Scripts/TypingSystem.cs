using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingSystem : MonoBehaviour
{
    public TMP_Text MyText;
    public string ToType = "";
    public float LastTimeTyping;
    public GameObject TextboxSystem;

    // Update is called once per frame
    void Update()
    {
        // If there is something to type and it has been over 7 milliseconds
        if (ToType != "" && LastTimeTyping > 0.07) {
            TextboxSystem.GetComponent<TextboxSystem>().state = 1;
            // Add the next letter
            MyText.text += ToType[0];
            // Delete the first letter from the string
            ToType = ToType[1..];
            // Reset the typing timer
            LastTimeTyping = 0;
        }
        // If there is nothing to type set state to 0
        if (ToType == "") {
            TextboxSystem.GetComponent<TextboxSystem>().state = 0;
        }
        // Update timer
        LastTimeTyping += Time.deltaTime;
    }
}
