using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit)) {
            transform.position = hit.point;
        }
    }
}
