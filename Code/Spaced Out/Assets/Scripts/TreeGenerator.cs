using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    public float yMax = 1600;
    public float xMax = 1500;
    public float yMin = -1800;
    public float xMin = -1900;
    public GameObject tree;
    public GameObject rock;
    public GameObject gem;
    public int currChunk;
    public GameObject chunkStorage;
    public int chunkSize;
    public List<GameObject> chunks = new List<GameObject>();
    public List<int> generatedChunks = new List<int>();
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTrees();
    }

    void GenerateTrees() {
        for (int i = 0; i < 5000; i++) {
            int choice = UnityEngine.Random.Range(1, 7);
            if (choice < 4) {
                SpawnTree(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax), chunkStorage);
            }
            if (choice > 3 && choice < 6) {
                SpawnRock(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax), chunkStorage);
            }
            if (choice > 5) {
                SpawnGem(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax), chunkStorage);
            }
        }
    }

    public void SpawnTree(float x, float y, GameObject parent){
        GameObject newTree = Instantiate(tree, new Vector3(x, 1000, y), Quaternion.Euler(-90, 0, 0));
        newTree.transform.parent = parent.transform;
    }

    public void SpawnRock(float x, float y, GameObject parent){
        GameObject newTree = Instantiate(rock, new Vector3(x, 1000, y), Quaternion.Euler(-90, 0, 0));
        newTree.transform.parent = parent.transform;
    }

    public void SpawnGem(float x, float y, GameObject parent){
        GameObject newTree = Instantiate(gem, new Vector3(x, 1000, y), Quaternion.Euler(-90, 0, 0));
        newTree.transform.parent = parent.transform;
    }
}
