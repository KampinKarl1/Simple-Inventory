using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory
{
    public class Inventory : MonoBehaviour
    {
        private Dictionary<Item, int> inventory = new Dictionary<Item, int>();

        private int maxItemSlots = 32;

        [SerializeField] private bool createItemDropOnRemove = false;
        [SerializeField] private Pickup pickupPrefab = null;

        #region Properties
        public Dictionary<Item, int> GetInventory => inventory;

        public bool HasNumberOfItem(Item item, int numNeeded) => inventory.ContainsKey(item) && inventory[item] >= numNeeded;

        public int NumberHeldOf(Item i) => inventory.ContainsKey(i)? inventory[i] : 0;
        public int GetMaxItemSlots => maxItemSlots;
        public int CountDifferentItems => inventory.Count;
        #endregion

        public delegate void OnInventoryChange();
        public OnInventoryChange onInventoryChange;

        private void Start()
        {
            foreach (var itemAndCount in FindObjectOfType<InventorySaveSystem>().LoadInventory()) 
            {
                inventory.Add(itemAndCount.Key, itemAndCount.Value);
            }
            
            onInventoryChange?.Invoke(); //Call the event so the UI and stuff gets updated

            if (useStartingItems)
            {
                if (startingItems != null &&
                    startingItems.Count > 0 &&
                    !System.Array.Exists(startingItems.ToArray(), element => element == null))
                {
                    foreach (Item i in startingItems)
                    {
                        AddToInventory(i, 1);
                    }
                }
            }
        }

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

        public void DropItem(Item item, int count) 
        {
            if (!HasNumberOfItem(item, count))
                return;

            RemoveFromInventory(item, count);

            if (createItemDropOnRemove && pickupPrefab != null)
            {
                //Pickup p = Instantiate(pickupPrefab, transform.position + transform.TransformDirection(new Vector3(0, 3f, 2f)), transform.rotation);
                Pickup p = Instantiate(item.Prefab != null ? item.Prefab.GetComponent<Pickup>() : pickupPrefab, 
                    transform.position + transform.TransformDirection(new Vector3(0, 3f, 2f)), 
                    transform.rotation);
                p.SetItem(item);
            }
        }
    }
}
