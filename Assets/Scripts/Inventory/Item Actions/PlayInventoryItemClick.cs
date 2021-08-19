using UnityEngine;
using UnityEngine.Events;

namespace SimpleInventory
{
    [CreateAssetMenu]
    public class PlayInventoryItemClick : ActionOnClickItemUI
    {
        public override UnityAction Action(Item i, int n, object inventory)
        {
            return new UnityAction(delegate
            {
                Inventory inv = inventory as Inventory;

                inv.RemoveFromInventory(i, n);

                i.DoItemStuff();
            });
        }
    }
}