using SimpleInventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace SimpleInventory
{
    public class ShopSlotUI : ItemSlotUI
    {
        [SerializeField] TextMeshProUGUI priceText = null;
        public override void UpdateSlotUI(Item i, int count, UnityAction buttonAction)
        {
            base.UpdateSlotUI(i, count, buttonAction);
            priceText.text = i.GetPrice().ToString();
        }
    }
}