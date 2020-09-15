using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace SimpleInventory.UI
{
    public class ShopUI : SlotHolderUI<Item>
    {
        [SerializeField] private Shopper shopper = null; //The user to whom this UI is shown

        private Shop shop = null;

        [SerializeField] private TextMeshProUGUI titleOfShop = null;

        void Start()
        {
            shopper.onEnterShop += SetShop;

            shopper.onExitShop += ExitShop;
        }

        private void SetShop(Shop shop) 
        {
            this.shop = shop;

            titleOfShop.text = shop.name;

            InitializeUI(shop.GetInventory, shop.MaxItems);

            ChangeInventoryState();

            UpdateItemSlots();

            shop.onInventoryChange += UpdateItemSlots;
        }

        private void ExitShop() 
        {
            shop.onInventoryChange -= UpdateItemSlots;

            ChangeInventoryState();
        }

        protected override void UpdateSlotAt(int _index, Item itemOfType, int count)
        {            
            itemSlots[_index].gameObject.SetActive(true);

            UnityAction buttonAction = new UnityAction(delegate {   
                shop.SellToPlayer(itemOfType);
                ;});

            itemSlots[_index].UpdateSlotUI(itemOfType, count, buttonAction);
        }
    }    
}