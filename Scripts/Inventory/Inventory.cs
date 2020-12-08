using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory
{
    public class Inventory : MonoBehaviour
    {
        private Dictionary<Item, int> inventory = new Dictionary<Item, int>();

        private int maxItemSlots = 32;

        public Dictionary<Item, int> GetInventory => inventory;

        public bool HasNumberOfItem(Item item, int numNeeded) => inventory.ContainsKey(item) && inventory[item] >= numNeeded;

        public int NumberHeldOf(Item i) => inventory[i];
        public int GetMaxItemSlots => maxItemSlots;
        public int CountDifferentItems => inventory.Count;

        public delegate void OnInventoryChange();
        public OnInventoryChange onInventoryChange;

        public void AddToInventory(Item item, int count = 1) 
        {
            if (!inventory.ContainsKey(item))
                inventory.Add(item, count);

            else
                inventory[item] += count;

            onInventoryChange?.Invoke();
        }

        public void RemoveFromInventory(Item item, int count) 
        {
            if (!inventory.ContainsKey(item))
                throw new System.Exception("The dictionary doesn't contain that item. How did we get here?");

            if (inventory[item] >= count)
                inventory[item] -= count;
            else 
            {
                Debug.LogWarning($"Inventory contains less that {count} of {item.name}. Setting count to zero");
                inventory[item] = 0;
            }

            onInventoryChange?.Invoke();
        }
    }
}