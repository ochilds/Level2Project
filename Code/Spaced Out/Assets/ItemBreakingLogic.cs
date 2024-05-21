using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBreakingLogic : MonoBehaviour
{
    public GameObject itemToDrop;
    public int amountToDrop;
    public String correctTool;
    public int hardness;
    public int strength;
    private void Start() {
        // Set strength to base hardness
        strength = hardness;
    }

    // Update is called once per frame
    void FixedUpdate() {
        // If strength is 0 or below spawn item and destroy
        if (strength <= 0) {
            for (int i = 0;i < amountToDrop;i++) {
                Instantiate(itemToDrop);
            }
            Destroy(this.gameObject);
        }
    }

    public void Break() {
        // Decrease strength by 1
        strength -= 1;
    }
}
