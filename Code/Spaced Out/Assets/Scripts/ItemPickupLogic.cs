using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemPickupLogic : MonoBehaviour
{
    public float height;
    public float heightOffset;
    public int amount;
    public GameObject player;
    public int id;
    public int pickupTimer;
    // Start is called before the first frame update
    void Start()
    {
        // Set player object
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Generate height value using sine
        height = (Mathf.Sin(Time.timeSinceLevelLoad * 2) + 3) * 0.05f;
        // Find distance between object and floor
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity)) {
            // If within 0.3 units snap to floor
            if (hit.distance < 1) {
                transform.position = new Vector3(transform.position.x, 
                                                transform.position.y + heightOffset + (height - hit.distance), 
                                                transform.position.z);
            }
            // Else move down
            else {
                transform.position = new Vector3(transform.position.x, 
                                                transform.position.y - 0.1f,
                                                transform.position.z);
            }
        }
        // If no floor is found move upwards
        else {
            transform.position = new Vector3(transform.position.x, 
                                            transform.position.y + 0.1f, 
                                            transform.position.z);
        }
        // Rotate object
        transform.Rotate(0, 0, 2);
        // Update Drop Timer
        pickupTimer -= 1;
        // If player has free slot and drop timer is up
        if (player.GetComponent<InventorySystem>().GetFirstEmptySlot() != -1 && pickupTimer <= 0) {
            // If within 3 units of player start moving towards player by averaging Vector3's with bias towards my position
            if (Vector3.Distance(transform.position, player.transform.position) < 3) {
                transform.position = (player.transform.position + (transform.position * 7)) / 8;
            }
            // If withing 0.75 units of player update players inventory and destory self if correctly added to inventory
            if (Vector3.Distance(transform.position, player.transform.position) < 1f) {
                if (player.GetComponent<InventorySystem>().AddItem(id, amount)) {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
