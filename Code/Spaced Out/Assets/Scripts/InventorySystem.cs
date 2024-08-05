using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySystem : MonoBehaviour
{
    public int[,] inventory = { { 0, 0, }, { 0, 0, }, { 0, 0, }, { 0, 0, }, { 0, 0, }, };
    public Dictionary<int, GameObject> inventoryItems = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> items = new Dictionary<int, GameObject>();
    public GameObject uiInventory;
    public GameObject inventoryItemStorage;
    public GameObject itemStorage;
    public GameObject[] slots;

    private void Start() {
        // Add all inventory items to the dictionary (Adding one so ID's match)
        for (int i = 1;i < inventoryItemStorage.transform.childCount + 1;i++) {
            inventoryItems[i] = inventoryItemStorage.transform.GetChild(i - 1).gameObject;
        }
        // Add all items to the dictionary (Adding one so ID's match)
        for (int i = 1;i < itemStorage.transform.childCount + 1;i++) {
            items[i] = itemStorage.transform.GetChild(i - 1).gameObject;
        }
        // Find all children of the UI object except for the camera,light and slection box and add to array
        slots = new GameObject[uiInventory.transform.childCount - 3];
        for (int i = 0; i < uiInventory.transform.childCount - 3;i++) {
            slots[i] = uiInventory.transform.GetChild(i).gameObject;
        }
    }

    private void FixedUpdate() {
        // For every internal slot
        for (int i = 0;i < inventory.GetLength(0);i++) {
            // If it holds an item
            if (inventory[i,0] != 0) {
                // Get the corresponding GameObject and instantiate as child of correct UI slot if no game object already exists
                if (slots[i].transform.childCount == 1) {
                    GameObject slotObject = inventoryItems[inventory[i,0]];
                    Instantiate(slotObject, slots[i].transform);
                }
                // Set text to correct amount
                slots[i].transform.GetChild(0).GetComponent<TextMesh>().text = inventory[i,1].ToString();
            }
        }
    }

    public bool AddItem(int item, int amount) {
        // Find if object is already in inventory
        int itemSlot = FindSlotWithItem(item);
        if (itemSlot != -1) {
            // If it is add the amount to the slot
            inventory[itemSlot,1] += amount;
            return true;
        }
        else {
            // Else find the first empty solt
            int firstSlot = GetFirstEmptySlot();
            if (firstSlot != -1) {
                // Set this slot to the correct id and amount
                inventory[firstSlot,0] = item;
                inventory[firstSlot,1] = amount;
                return true;
            }
            else {
                // If there are no empty slots fail
                return false;
            }
        }
    }

    public bool RemoveItemFromSlot(int slot) {
        if (inventory[slot,1] == 0) {
            return false;
        }
        if (inventory[slot,1] == 1) {
            inventory[slot,0] = 0;
            inventory[slot,1] = 0;
            return true;
        }
        if (inventory[slot,1] > 1) {
            inventory[slot,1] -= 1;
            return true;
        }
        return false;
    }

    public int FindSlotWithItem(int item) {
        // For every slot in inventory
        for (int i = 0; i < (inventory.Length / 2) - 1; i++) {
            // If it is the correct id return the slot number
            if (inventory[i,0] == item) {
                return i;
            }
        }
        // If no slots are found return -1
        return -1;
    }

    public int GetFirstEmptySlot() {
        // For every slot in inventory
        for (int i = 0; i < (inventory.Length / 2); i++) {
            // If it is 0 return the slot number
            if (inventory[i,0] == 0) {
                return i;
            }
        }
        // If no slots are found return -1
        return -1;
    }

    public void DropItem(int index, int slot) {
        // If successfully removes object from inventory
        if (RemoveItemFromSlot(slot)) {
            // Create new item and set object pickup timer to 100 frames (1.4 seconds)
            GameObject droppedItem = (GameObject)Instantiate(items[index], transform.position + transform.forward, Quaternion.Euler(-90, 0, 0));  
            droppedItem.GetComponent<ItemPickupLogic>().pickupTimer = 100;
        }
    }
}
