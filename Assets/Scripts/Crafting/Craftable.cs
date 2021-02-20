using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory.Crafting
{
    [CreateAssetMenu (menuName = "Crafting/New Craftable")]
    public class Craftable : Item
    {
        [System.Serializable]
        public struct CraftingIngredient 
        {
            public Item ingredient;
            public int numberNeeded;

            public CraftingIngredient(Item i, int number) 
            {
                ingredient = i;
                numberNeeded = number;
            }
        }

        [SerializeField] private CraftingIngredient[] ingredients = null;

        public int NumberOfIngredients => ingredients.Length;
        public Item GetIngredientAt(int index) => ingredients[index].ingredient;
        public int GetNumberNeededAt(int index) => ingredients[index].numberNeeded;

        public void AddIngredient(Item i, int number) 
        {
            CraftingIngredient[] copy = new CraftingIngredient[ingredients.Length + 1];
            ingredients.CopyTo(copy, 0);

            copy[ingredients.Length + 1] = new CraftingIngredient(i, number);

            ingredients = copy;
        }
    }
}