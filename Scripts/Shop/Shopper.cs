using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory
{
    [RequireComponent(typeof(Wallet))]
    public class Shopper : MonoBehaviour
    {
        [SerializeField] Inventory inventory = null;
        private Wallet wallet = null;

        private Shop currentShop = null;

        public Shop GetCurrentShop => currentShop;

        public delegate void OnEnterShop(Shop shop);
        public delegate void OnExitShop();
        public OnEnterShop onEnterShop;
        public OnExitShop onExitShop;

        #region Builtin
        private void Start()
        {
            if (!TryGetComponent(out wallet))
                throw new System.Exception(name + " is a shopper that needs a wallet component. Make sure " + gameObject.name +
                    " has a wallet component.");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Shop"))
            {
                Shop s;
                if (other.TryGetComponent(out s))
                    EnterShop(s);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Shop"))
                ExitShop();
        }
        #endregion

        private void EnterShop(Shop s) 
        {
            currentShop = s;

            currentShop.EnterShop(this);

            onEnterShop?.Invoke(s);
        }

        private void ExitShop() 
        {
            currentShop.ExitShop();

            currentShop = null;
           
            onExitShop?.Invoke();
        }

        public bool TryBuyItem(Item i, int number) 
        {
            if (wallet.CanAfford(i.GetPrice()))
            {
                inventory.AddToInventory(i, number);

                wallet.RemoveMoney(i.GetPrice() * number);

                return true;
            }
            return false;
        }

        public void SellItem(Item i) 
        {
            //If at a shop, we have the item, and the shop can afford/will be able to resell it
            if (currentShop && inventory.HasNumberOfItem(i, 1) && currentShop.TryBuyFromPlayer(i))
            {
                inventory.RemoveFromInventory(i, 1);

                wallet.AddMoney(i.GetPrice());
            }
        }
    }
}