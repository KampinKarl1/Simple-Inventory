using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleInventory.UI
{
    public abstract class SlotHolderUI <T> : MonoBehaviour
    {
        protected Dictionary<T, int> inventoryOfType;

        protected ItemSlotUI[] itemSlots = null;
        [SerializeField] private GameObject slotUIPrefab = null;

        [SerializeField] private GameObject holderObject = null;
        [SerializeField] private Transform contentParent = null;

        public virtual void InitializeUI (object inventoryHolder, Dictionary<T,int> inventory, int numSlots)
        {
            inventoryOfType = inventory;

            CreateItemSlots(numSlots);

            UpdateItemSlots();
        }

        private void CreateItemSlots(int numSlotsToCreate)
        {
            itemSlots = new ItemSlotUI[numSlotsToCreate];

            for (int i = 0; i < numSlotsToCreate; ++i)
            {
                GameObject obj = Instantiate(slotUIPrefab, contentParent);

                itemSlots[i] = obj.GetComponent<ItemSlotUI>();
            }
        }

        /// <summary>
        /// Iterate on itemslots for which an item exists. Deactivate the unoccupied itemslots.
        /// </summary>
        /// <param name="inventory"></param>
        protected void UpdateItemSlots()
        {
            int index = 0;

            foreach (KeyValuePair<T, int> kvp in inventoryOfType) 
            {
                UpdateSlotAt(index, kvp.Key, kvp.Value);
                index++;
            }

            for (int i = index; i < itemSlots.Length; ++i) //Deactivate unoccupied slots
                itemSlots[i].gameObject.SetActive(false);
        }

        //Let polymorphs decide what to do with occupied containers based on type
        protected abstract void UpdateSlotAt(int _index, T itemOfType, int count);

        public void ChangeInventoryState() => holderObject.SetActive(!holderObject.activeSelf);
    }
}