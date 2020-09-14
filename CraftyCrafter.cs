using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory.Crafting 
{
    public class CraftyCrafter : MonoBehaviour
    {
        [SerializeField] private Inventory inventory = null;

        private int maxRecipeSlots = 12;

        [SerializeField, Tooltip("The technologies/recipes/craftables that this class currently knows"), Header("Technologies Known")]
        private List<Craftable> craftables = new List<Craftable>();

        private Dictionary<Craftable, int> craftingInventory = new Dictionary<Craftable, int>();
        public Dictionary<Craftable, int> CraftingInventory => craftingInventory;

        public int GetMaxRecipeSlots => maxRecipeSlots;

        public delegate void OnCraftablesChange();
        public OnCraftablesChange onCraftablesChange;

        private void Start()
        {
            foreach (Craftable c in craftables)
                AddToCraftables(c);

            inventory.onInventoryChange += FindNumCraftableForAllRecipes;
        }

        private void AddToCraftables(Craftable c, int count = 0) 
        {
            if (!craftingInventory.ContainsKey(c))
                craftingInventory.Add(c, count);
            else
                craftingInventory[c] += count;
        }


        //Called when there is a change to the inventory (Since that would possibly change the number of items we can craft)
        void FindNumCraftableForAllRecipes()
        {
            Craftable c;

            for (int i = 0; i < craftables.Count; ++i)
            {
                c = craftables[i];

                if (!craftingInventory.ContainsKey(c))
                    AddToCraftables(c);

                craftingInventory[c] = NumberCraftable(c);
            }

            onCraftablesChange?.Invoke();
        }

        //Will not find the number craftable for more complex items when the ingredients are not already in inventory
        //ex. A House will show 0 craftable unless Door, Roof, and Walls(4) are ALREADY in inventory, even if the base materials for the components ARE in inventory.
        public int NumberCraftable(Craftable craftable)
        {
            //Find the max number craftable given a desired item

            int lowest = int.MaxValue;

            for (int i = 0; i < craftable.NumberOfIngredients; ++i)
            {
                Item ingredient = craftable.GetIngredientAt(i);
                int numNeeded = craftable.GetNumberNeededAt(i);

                if (inventory.HasNumberOfItem(ingredient, numNeeded))
                {
                    int numCraftable = inventory.NumberHeldOf(ingredient) / numNeeded;

                    if (numCraftable < lowest)
                        lowest = numCraftable;
                }
                else //There isnt enough of the ingredient, therefore 0 can be made
                    return 0;
            }

            return lowest;
        }
        public bool TryCraftItem(Craftable item)
        {
            if (!CanCraftItem(item))
            {
                Debug.LogWarning("Not enough materials to craft " + item.ItemName);
                return false;
            }

            Item craftedItem = CraftItem(item);

            if (craftedItem != null) 
            {
                inventory.AddToInventory(craftedItem);
                return true;
            }

            return false;
        }

        private bool CanCraftItem(Craftable craftable)
        {
            Item ingredient;
            int numNeeded;

            for (int i = 0; i < craftable.NumberOfIngredients; ++i)
            {
                ingredient = craftable.GetIngredientAt(i);
                numNeeded = craftable.GetNumberNeededAt(i);

                if (!inventory.HasNumberOfItem(ingredient, numNeeded))
                    return false;
            }

            return true;
        }

        private Item CraftItem(Craftable craftable)
        {
            for (int i = 0; i < craftable.NumberOfIngredients; i++)
                inventory.RemoveFromInventory(craftable.GetIngredientAt(i), craftable.GetNumberNeededAt(i));

            return craftable;
        }
        
        public void LearnNewCraftable(Craftable c) 
        {
            craftables.Add(c);

            AddToCraftables(c);

            onCraftablesChange?.Invoke();
        }

        private void OnValidate()
        {
            foreach (Craftable c in craftables)
                AddToCraftables(c); //Wont add those that already exist

            onCraftablesChange?.Invoke();
        }
    } 
}
