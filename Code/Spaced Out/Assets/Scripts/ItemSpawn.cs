using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{

    void Start()
    {
        // Move to the floor
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit)) {
            transform.position = hit.point;
        }
    }
}
