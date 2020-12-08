using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory
{
    [RequireComponent(typeof(Wallet))]
    public class Shop : Inventory
    {
        private Wallet wallet = null;

        private Shopper shopper = null;

        [SerializeField] private List<Item> shopItems = new List<Item>();
        [SerializeField] private int numberToAdd = 5;

        private int maxItems = 12;
        public int MaxItems => maxItems;

        public delegate void OnEnterShop();
        public OnEnterShop onEnterShop;

        public delegate void OnExitShop();
        public OnExitShop onExitShop;

        private void Start()
        {
            if (!TryGetComponent(out wallet))
                throw new System.Exception(name + " is a shopper that needs a wallet component. Make sure " + gameObject.name +
                    " has a wallet component.");

            foreach (Item i in shopItems)
                AddToInventory(i, numberToAdd);
        }

        public void EnterShop(Shopper s) 
        { 
            shopper = s;
            onEnterShop?.Invoke();
        }

        public void ExitShop() 
        { 
            shopper = null;
            onExitShop?.Invoke();
        }

        #region Selling
        /// <summary>
        /// Shop is selling to the player.
        /// </summary>
        /// <param name="i"></param>
        public void SellToPlayer(Item i) 
        {
            if (!shopper || !HasNumberOfItem(i, 1))
                return;

            if (shopper.TryBuyItem(i, 1))
            {
                RemoveFromInventory(i, 1);

                wallet.AddMoney(i.GetPrice());
            }
        }
        #endregion

        #region Buying
        /// <summary>
        /// Shop is buying from the player.
        /// </summary>
        /// <param name="i"></param>
        public bool TryBuyFromPlayer(Item i)
        {
            if (CanBuy(i))
            {
                wallet.RemoveMoney(i.GetPrice());
                AddToInventory(i, 1);
                return true;
            }
            return false;
        }

        private bool CanBuy(Item i)
        {
            //Can sell type of item or whatever

            return wallet.CanAfford(i.GetPrice());
        }
        #endregion
    }
}