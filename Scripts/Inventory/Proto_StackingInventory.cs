using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace SimpleInventory
{
    public class Proto_StackingInventory : MonoBehaviour
    {
        private Dictionary<Item, float> stackSizes = new Dictionary<Item, float>();
        private Dictionary<StackableItem, int> stackableInventory = new Dictionary<StackableItem, int>();
        public void AddStackableItem(StackableItem item, int amount)
        {
            if (!stackableInventory.ContainsKey(item))
            {
                stackableInventory.Add(item, 0);
                stackSizes.Add(item, 0); //If it's not in the stack inventory, it's not in the stacksize inventory.
            }

            //'amount' can be greater than an item's stack size limit. Ex. We have wood with maxStacks of 10 - We add 15 wood, so 
            //  the normal inventory gets one stack of ten plus one stack of five.
            float numStacksToAdd = (float) amount / (float) item.stackSize; //Looks stupid and unnecesary, but amount and stack size must be parsed to float
            Debug.LogWarning("Stacks to add " + numStacksToAdd.ToString());
            Debug.LogWarning("Stacks to add (int) " + ((int)numStacksToAdd).ToString());
            //Counting the number of each item - If 'num' is non one, non zero, add one (so it's not parsed to zero) - otherwise add the number.
            stackableInventory[item] += (int) numStacksToAdd;//numStacksToAdd < 1 && numStacksToAdd > 0 ? 1 : (int)numStacksToAdd;

            //Set the number of items being added to the stack as just the partial stack
            float partialStack = numStacksToAdd - (int)numStacksToAdd;            

            AddToStack(item, partialStack);
        }

        private void AddToStack(StackableItem item, float num) 
        {
            stackSizes[item] += num;

            //If a stack is completed, add another full-stack instance of the item to the inventory and remove a stack from the stack fraction counter.
            if (stackSizes[item] > 1)
            {
                stackableInventory[item]++;
                stackSizes[item] -= 1.0f;
            }
        }

        public string NumberOfStacks_TotalCount_AndPartialStackCountForItem(StackableItem item)
        {
            StringBuilder sb = new StringBuilder(item.name + "\n");

            int numStacks = stackableInventory[item];
            int itemsInPartialStack = (int)(stackSizes[item] * (float)item.stackSize);
            int totalItems = (numStacks * item.stackSize) + itemsInPartialStack;

            if (stackSizes[item] > 0)
                numStacks++; //Account for partial stack

            sb.Append("Number of Stacks: " + numStacks) ;
            sb.Append("\nTotal Item Count: " + totalItems);
            sb.Append("\nIncomplete stack size: " + stackSizes[item]);

            return sb.ToString();
        }

        public class StackableItem : Item 
        {
            public int stackSize = 10;

            public void InitializeItem(string _name, int _stackSize)
            {
                name = _name;
                stackSize = _stackSize;
            }
        }

        private void Start()
        {
            StackableItem logPile = ScriptableObject.CreateInstance<StackableItem>();
            logPile.InitializeItem("Log Pile", 10);

            AddStackableItem(logPile, 25);

            print (NumberOfStacks_TotalCount_AndPartialStackCountForItem(logPile));
        }
    }
}