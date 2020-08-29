using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using SimpleInventory.Crafting;

namespace SimpleInventory.UI
{
    public class CraftingUI : SlotHolderUI <Craftable>
    {
        private CraftyCrafter crafter = null;

        public override void InitializeUI(object inventoryHolder, Dictionary<Craftable, int> inventory, int numSlots)
        {
            crafter = inventoryHolder as CraftyCrafter;

            base.InitializeUI(inventoryHolder, inventory, numSlots);

            crafter.onCraftablesChange += UpdateItemSlots;
        }

        /*protected override void UpdateItemSlots() 
        {
            UnityAction craftAction;

            int index = 0;

            foreach (KeyValuePair<Craftable, int> kvp in crafter.CraftingInventory)
            {
                Craftable item = kvp.Key;
                int count = kvp.Value;

                craftAction = new UnityAction(delegate
                {
                   crafter.TryCraftItem(item);
                });

                itemSlots[index].gameObject.SetActive(true);
                itemSlots[index].UpdateSlotUI(item, count, craftAction);

                index++;
            }

            for (int i = index; i < itemSlots.Length; i++)
                itemSlots[i].gameObject.SetActive(false);
        }*/

        protected override void UpdateSlotAt(int _index, Craftable i, int count)
        {
            itemSlots[_index].gameObject.SetActive(true); //If theres a craftable, show it even if the count is currently 0

            UnityAction buttonAction = new UnityAction(delegate
            {
               crafter.TryCraftItem(i);
            });

            itemSlots[_index].UpdateSlotUI(i, count, buttonAction);
        }
    }
}