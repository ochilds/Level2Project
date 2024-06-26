using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingSystem : MonoBehaviour
{
    public Text MyText;
    public string ToType = "";
    public float LastTimeTyping;

    // Update is called once per frame
    void Update()
    {
        // If there is something to type and it has been over 5 milliseconds
        if (ToType != "" && LastTimeTyping > 0.05) {
            // Add the next letter
            MyText.text += ToType[0];
            // Delete the first letter from the string
            ToType = ToType[1..];
            // Reset the typing timer
            LastTimeTyping = 0;
        }
        // Update timer
        LastTimeTyping += Time.deltaTime;
    }
}
