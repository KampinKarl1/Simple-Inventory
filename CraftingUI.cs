using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SimpleInventory.Crafting;

namespace SimpleInventory.UI
{
    public class CraftingUI : SlotHolderUI <Craftable>
    {
          [SerializeField] private CraftyCrafter crafter = null;

        private void Start()
        {
            if (crafter)
                InitializeUI(crafter.CraftingInventory, crafter.GetMaxRecipeSlots);
        }

        protected override void InitializeUI(Dictionary<Craftable, int> inventory, int numSlots)
        {
            base.InitializeUI(inventory, numSlots);

            UpdateItemSlots();

            crafter.onCraftablesChange += UpdateItemSlots;
        }

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
