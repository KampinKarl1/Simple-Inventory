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
        [SerializeField] private CraftyCrafter crafter = null;

        private void Start()
        {
            if (crafter)
                InitializeUI(crafter.CraftingInventory, crafter.GetMaxRecipeSlots);
        }

        protected override void InitializeUI(Dictionary<Craftable, int> inventory, int numSlots)
        {
            base.InitializeUI(inventory, numSlots);

            CacheTooltips();

            UpdateItemSlots();

            crafter.onLearnNewCraftable += AddCraftableRequirementsMessage;
            crafter.onCraftablesChange += UpdateItemSlots;
        }

        protected override void UpdateSlotAt(int _index, Craftable craftable, int count)
        {
            itemSlots[_index].gameObject.SetActive(true); //If theres a craftable, show it even if the count is currently 0

            if (!craftableRequirements.ContainsKey(craftable))
                AddCraftableRequirementsMessage(craftable);

            if (tooltips[_index] && craftable) 
                tooltips[_index].SetMessage(craftableRequirements[craftable]);            

            UnityAction buttonAction = new UnityAction(delegate {
                crafter.TryCraftItem(craftable);
                ;});

            itemSlots[_index].UpdateSlotUI(craftable, count, buttonAction);
        }

        #region Tooltips
        private Dictionary<Craftable, string> craftableRequirements = new Dictionary<Craftable, string>();

        private Tooltipper[] tooltips = null;
        private void CacheTooltips()
        {
            tooltips = new Tooltipper[itemSlots.Length];

            for (int i = 0; i < itemSlots.Length; i++)
                tooltips[i] = itemSlots[i].GetComponent<Tooltipper>();
        }

        private void AddCraftableRequirementsMessage(Craftable c) 
        {
            string message = c.Description + "\n \n Requires: ";

            for (int index = 0; index < c.NumberOfIngredients; ++index)
            {
                message += $"\n {c.GetIngredientAt(index).ItemName} x {c.GetNumberNeededAt(index)}";
                index++;
            }

            craftableRequirements.Add(c, message);
        }
        #endregion
    }
}
