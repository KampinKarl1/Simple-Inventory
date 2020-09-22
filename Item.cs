using UnityEngine;

namespace SimpleInventory
{
    [CreateAssetMenu]
    public class Item : ScriptableObject, IExchangeable<int>
    {
        [SerializeField] private string itemName = "Write a name";
        [SerializeField, Multiline] private string description = "does stuff";
        [SerializeField] private Sprite icon = null;
        [SerializeField] private int value = 1;

        public Sprite Icon => icon;
        public string ItemName => itemName;
        public string Description => description;

        public void DoItemStuff() 
        {
            //Make the item do stuff here
        }
        
        //Implementation of IExchangeable
        public int GetPrice() 
        {
            return value;
        }
    }
}
