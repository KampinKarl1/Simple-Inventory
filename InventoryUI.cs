using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleInventory.UI
{
    public class InventoryUI : SlotHolderUI <Item>
    {
        [SerializeField] private Inventory inventory = null;

        private void Start()
        {
            if (inventory)
                InitializeUI(inventory.GetInventory, inventory.GetMaxItemSlots);
        }

        protected override void InitializeUI(Dictionary<Item,int> inventory, int numSlots)
        {
            base.InitializeUI(inventory, numSlots);

            this.inventory.onInventoryChange += UpdateItemSlots;
        }


        protected override void UpdateSlotAt(int _index, Item item, int count)
        {
            if (count == 0)
            {
                itemSlots[_index].gameObject.SetActive(false);
                return;
            }

            UnityAction buttonAction = new UnityAction(delegate
            {
                inventory.RemoveFromInventory(item, 1);
                item.DoItemStuff();
            });

            itemSlots[_index].gameObject.SetActive(true);
            itemSlots[_index].UpdateSlotUI(item, count, buttonAction);
        }
    }
}
