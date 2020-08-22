using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        private Inventory inventory = null;

        [SerializeField] int numSlotsToCreate = 15;

        [SerializeField] ItemSlotUI [] itemSlots = null;
        [SerializeField] GameObject slotUIPrefab = null;

        [SerializeField] private GameObject inventoryUIobj = null;
        [SerializeField] private Transform contentParent = null;

        public void InitializeUI(Inventory inventory)
        {
            this.inventory = inventory;

            CreateItemSlots();

            UpdateItemSlots();

            inventory.onInventoryChange += UpdateItemSlots;
        }

        private void CreateItemSlots() 
        {
            itemSlots = new ItemSlotUI[numSlotsToCreate];

            for (int i = 0; i < numSlotsToCreate; ++i)
            {
                GameObject obj = Instantiate(slotUIPrefab);
                obj.transform.SetParent(contentParent);

                itemSlots[i] = obj.GetComponent<ItemSlotUI>();
                itemSlots[i].Initialize();
            }
        }

        private void UpdateItemSlots() 
        {
            int i = 0;

            foreach (KeyValuePair<Item, int> kvp in inventory.GetInventory)
            {
                if (kvp.Value == 0)
                    itemSlots[i].gameObject.SetActive(false);
                else
                {
                    itemSlots[i].gameObject.SetActive(true);
                    itemSlots[i].UpdateSlotUI(this, kvp.Key, kvp.Value);
                }
                i++;
            }

            for (int j = i; j < itemSlots.Length; j++)
                itemSlots[j].gameObject.SetActive(false);            
        }

        public void ItemClicked(Item item) 
        {
            inventory.RemoveFromInventory(item, 1);
        }

        public void ChangeInventoryState() => inventoryUIobj.SetActive(!inventoryUIobj.activeSelf);
    }
}