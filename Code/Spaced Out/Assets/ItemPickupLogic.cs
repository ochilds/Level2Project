using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemPickupLogic : MonoBehaviour
{
    public float height;
    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
