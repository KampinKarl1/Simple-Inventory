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

        public delegate void OnInit();
        public OnInit onInit;

        public delegate void OnCraftablesChange();
        public OnCraftablesChange onCraftablesChange;

        public delegate void OnLearnNewCraftable(Craftable craftable);
        public OnLearnNewCraftable onLearnNewCraftable;

        private void Start()
        {
            foreach (Craftable c in craftables)
                AddToCraftables(c);

            inventory.onInventoryChange += FindNumCraftableForAllRecipes;
        }

        private void AddToCraftables(Craftable c, int count = 0) 
        {
            if (!craftingInventory.ContainsKey(c))
            {
                craftingInventory.Add(c, count);
                onLearnNewCraftable?.Invoke(c);
            }
            else
                craftingInventory[c] += count;
        }


        //Called when there is a change to the inventory (Since that would possibly change the number of items we can craft)

        //Making this into a class var so it doesn't cause mem fragmentation
        //be careful to nullify every iteration in FindNumCraftable
        Craftable craftable = null;
        void FindNumCraftableForAllRecipes()
        {
            //Craftable craftable;

            for (int i = 0; i < craftables.Count; ++i)
            {
                craftable = craftables[i];

                if (!craftingInventory.ContainsKey(craftable))
                    AddToCraftables(craftable);

                craftingInventory[craftable] = GetNumberCraftable(craftable);

                craftable = null; //This doesn't have to be done, but it's probably safer so this method doesn't reference the last craftable from the last call
            }

            onCraftablesChange?.Invoke();
        }

        //Will not find the number craftable for more complex items when the ingredients are not already in inventory
        //ex. A House will show 0 craftable unless Door, Roof, and Walls(4) are ALREADY in inventory, even if the base materials for the components ARE in inventory.
        public int GetNumberCraftable(Craftable craftable)
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

        //Alloc for crafted item - make sure to nullify it after crafting
        Item craftedItem = null;

        public bool TryCraftItem(Craftable item)
        {
            if (!CanCraftItem(item))
            {
                //Ideally you'd have some sort of affordance for the player to let them know that they can't craft the thing they just tried to make
                Debug.LogWarning("Not enough materials to craft " + item.ItemName);
                return false;
            }

            craftedItem = CraftItem(item); //This was previously a local var, not a class field.

            if (craftedItem != null) 
            {
                inventory.TryAddToInventory(craftedItem);
                //Nullify crafted item so it's not referenced it the next time this is called.
                craftedItem = null;

                return true;
            }

            return false;
        }

        private bool CanCraftItem(Craftable craftable)
        {
            for (int i = 0; i < craftable.NumberOfIngredients; ++i)
            {
                if (!inventory.HasNumberOfItem(craftable.GetIngredientAt(i), craftable.GetNumberNeededAt(i)))
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
