using UnityEngine;
using UnityEngine.Events;

namespace SimpleInventory
{
    [CreateAssetMenu]
    public class SellToShopAction : ItemAction
    {
        public override UnityAction Action(Item i, int n, object player)
        {
            return new UnityAction(delegate
            {
                Inventory inventory = player as Inventory;

                Shopper s = inventory.gameObject.GetComponent<Shopper>();

                s.SellItem(i);
            });
        }
    }
}