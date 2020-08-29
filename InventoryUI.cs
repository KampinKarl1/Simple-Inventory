using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleInventory.UI
{
    public class InventoryUI : SlotHolderUI <Item>
    {
        private Inventory inventory = null;
        public override void InitializeUI(object inventoryHolder, Dictionary<Item,int> inventory, int numSlots)
        {
            this.inventory = inventoryHolder as Inventory;

            base.InitializeUI(inventoryHolder, inventory, numSlots);

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

        /*protected override void UpdateItemSlots()
        {
            UnityAction buttonAction;

            int index = 0;
            //Iterate on each slot
            foreach (KeyValuePair<Item, int> kvp in inventory.GetInventory)
            {
                Item item = kvp.Key;
                int count = kvp.Value;

                if (count == 0)
                {
                    itemSlots[index].gameObject.SetActive(false);
                    continue;
                }


                buttonAction = new UnityAction(delegate
               {
                   inventory.RemoveFromInventory(item, 1);
                   item.DoItemStuff(); 
               });

                itemSlots[index].UpdateSlotUI(item, count, buttonAction);
                itemSlots[index].gameObject.SetActive(true);

                index++;
            }

            for (int i = index; i < itemSlots.Length; i++)            
                itemSlots[i].gameObject.SetActive(false);           
        }*/


    }
}