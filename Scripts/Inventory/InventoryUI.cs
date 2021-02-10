using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleInventory.UI
{
    public class InventoryUI : SlotHolderUI <Item>
    {
        [SerializeField] private Inventory inventory = null;

        [SerializeField] private ItemAction itemAction;

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


        //There is an absolutely massive error with this if there aren't enough item slots created. 
        //The slots are iterated over with the inventory dictionary on this script as opposed to using the inventory dictionary on the player
        //Need to disable slots without items and not create the aforementioned dependency.
        protected override void UpdateSlotAt(int _index, Item item, int count)
        {
            if (count == 0)
                itemSlots[_index].gameObject.SetActive(false);
            else
            {
                itemSlots[_index].gameObject.SetActive(true);

                SetTooltipMessage(_index, item.Description);

                UnityAction buttonAction = itemAction.Action(item, 1, inventory);
               
                itemSlots[_index].UpdateSlotUI(item, count, buttonAction);
            }
        }

        private void SetTooltipMessage(int index, string message) 
        {
            Tooltipper t;
            if (itemSlots[index].TryGetComponent(out t))
                t.SetMessage(message);
        }
    }
}
