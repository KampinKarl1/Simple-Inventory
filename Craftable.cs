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
        }

        [SerializeField] private CraftingIngredient[] ingredients = null;

        public int NumberOfIngredients => ingredients.Length;
        public Item GetIngredientAt(int index) => ingredients[index].ingredient;
        public int GetNumberNeededAt(int index) => ingredients[index].numberNeeded;
    }
}
