using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySystem : MonoBehaviour
{
    public int[,] inventory = { { 0, 0, }, { 0, 0, }, { 0, 0, }, { 0, 0, }, { 0, 0, }, };

    public bool AddItem(int item, int amount) {
        int itemSlot = FindSlotWithItem(item);
        if (itemSlot != -1) {
            inventory[itemSlot,1] += amount;
            return true;
        }
        else {
            int firstSlot = GetFirstEmptySlot();
            if (firstSlot != -1) {
                inventory[firstSlot,0] = item;
                inventory[firstSlot,1] = amount;
                return true;
            }
            else {
                return false;
            }
        }
    }

    public int FindSlotWithItem(int item) {
        for (int i = 0; i < (inventory.Length / 2) - 1; i++) {
            if (inventory[i,0] == item) {
                return i;
            }
        }
        return -1;
    }

    public int GetFirstEmptySlot() {
        for (int i = 0; i < (inventory.Length / 2); i++) {
            if (inventory[i,0] == 0) {
                return i;
            }
        }
        return -1;
    }
}
