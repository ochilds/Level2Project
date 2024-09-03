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
    public int shakeLength;
    public int shakeTimer;
    public Vector3 toMove = Vector3.zero;
    public GameObject breakParticle;
    private void Start() {
        // Set strength to base hardness
        strength = hardness;
    }

    // Update is called once per frame
    void FixedUpdate() {
        // Move if needed to create shake effect
        transform.position -= toMove;
        toMove = Vector3.zero;
        // Get new movement for next frame
        if (shakeTimer > 0) {
            toMove = new Vector3(UnityEngine.Random.Range(-0.03f, 0.03f), 
                                 0, 
                                 UnityEngine.Random.Range(-0.03f, 0.03f));
            transform.position += toMove;
            shakeTimer -= 1;
        }
        // If strength is 0 or below spawn item with random amount within range and destroy
        if (strength <= 0) {
            GameObject item = Instantiate(itemToDrop, transform.position, transform.rotation);
            item.GetComponent<ItemPickupLogic>().amount = UnityEngine.Random.Range(minAmountToDrop, maxAmountToDrop);
            Destroy(gameObject);
        }
    }

    public void Break(Vector3 position, Vector3 rotation) {
        // Decrease strength by 1
        strength -= 1;
        shakeTimer = shakeLength;
        Instantiate(breakParticle, position, Quaternion.Euler(rotation));
    }
}
