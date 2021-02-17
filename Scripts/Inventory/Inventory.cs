using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory
{
    public class Inventory : MonoBehaviour
    {
        private Dictionary<Item, int> inventory = new Dictionary<Item, int>();

        [SerializeField, Header ("Inventory Attributes"), Tooltip("The maximum number of unique items in the inventory")]
        private int maxItemSlots = 32;

        [Space]
        [Header("Debugging for other designers")]
        [SerializeField, Tooltip("Checks to see if the inventory has presented a fix dialogue to the user if they didn't enable pickups on drop")]
        private SharedBool presentedPickupFix = null;

        [SerializeField] private bool createItemDropOnRemove = false;
        [SerializeField] private Pickup pickupPrefab = null;
        [SerializeField, Tooltip("Where items are placed when dropped")] 
        private Vector3 itemDropOffset = new Vector3(0, 3, 2); //Where, in relation to the inventory user, will items be dropped

        #region Properties
        public Dictionary<Item, int> GetInventory => inventory;

        /// <summary>
        /// Check if the inventory has a given item and a given number of that item.
        /// </summary>
        /// <param name="item">Check if the inventory has this item.</param>
        /// <param name="numNeeded">Check if the inventory has at least this number of the item.</param>
        /// <returns>True if has the item in given quantity.</returns>
        public bool HasNumberOfItem(Item item, int numNeeded) => inventory.ContainsKey(item) && inventory[item] >= numNeeded;

        public int NumberHeldOf(Item i) => inventory.ContainsKey(i)? inventory[i] : 0;
        public int GetMaxItemSlots => maxItemSlots;
        public int CountDifferentItems ()//=> inventory.Count;
        {
            int diffItems = 0;

            foreach (KeyValuePair<Item, int> kvp in inventory) 
            {
                if (kvp.Value > 0) //If there is at least one of the item in the inventory
                    diffItems++;
            }

            return diffItems;
        }
        #endregion

        public delegate void OnInventoryChange();
        public OnInventoryChange onInventoryChange;

        public bool TryAddToInventory(Item item, int count = 1) 
        {
            if (!inventory.ContainsKey(item))
            {
                //If the inventory doesnt have the item and is full, dont add it
                if (CountDifferentItems() >= maxItemSlots)
                    return false;

                inventory.Add(item, count);
            }
            else
                inventory[item] += count;

            onInventoryChange?.Invoke();

            return true;
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
                    transform.position + transform.TransformDirection(itemDropOffset),
                    transform.rotation);
                p.SetItem(item);
            }
            else if (!createItemDropOnRemove && presentedPickupFix.Value == false) 
            {
                Debug.Break();
                Debug.LogWarning("Did you mean to drop a physical item into the world when removing from Inventory? " +
                    "You don't have drops enabled on the Inventory component. Please tick the createItemDropOnRemove box on the inventory and " +
                    "make sure you have a pickupPrefab object on the Inventory. That prefab must have a Pickup component on it." +
                    "Press 'Play' to disable pause. This error will only show once.");
                Debug.LogWarning("LOOK AT THE CONSOLE PLEASE.");
                presentedPickupFix.SetValue(true);
            }
        }
    }
}