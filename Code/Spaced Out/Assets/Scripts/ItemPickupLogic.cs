using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemPickupLogic : MonoBehaviour
{
    public float height;
    public int amount;
    public GameObject player;
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Generate height value using sine
        height = (Mathf.Sin(Time.timeSinceLevelLoad * 2) + 3) * 0.05f;
        //Find distance between object and floor and adjust position accordinly
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity)) {
            transform.position = new Vector3(transform.position.x, transform.position.y + (height - hit.distance), transform.position.z);
        }
        // Rotate object
        transform.Rotate(0, 0, 2);
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
