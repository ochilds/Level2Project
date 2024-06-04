using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBreakingLogic : MonoBehaviour
{
    public GameObject itemToDrop;
    public int minAmountToDrop;
    public int maxAmountToDrop;
    public String correctTool;
    public int hardness;
    public int strength;
    private void Start() {
        // Set strength to base hardness
        strength = hardness;
    }

    // Update is called once per frame
    void FixedUpdate() {
        // If strength is 0 or below spawn item with random amount within range and destroy
        if (strength <= 0) {
            GameObject item = Instantiate(itemToDrop, transform.position, transform.rotation);
            item.GetComponent<ItemPickupLogic>().amount = UnityEngine.Random.Range(minAmountToDrop, maxAmountToDrop);
            Destroy(gameObject);
        }
    }

    public void Break() {
        // Decrease strength by 1
        strength -= 1;
    }
}
