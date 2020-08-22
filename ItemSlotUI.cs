using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SimpleInventory.UI
{
    public class ItemSlotUI : MonoBehaviour
    {
        [SerializeField] private Button itemButton = null;
        [SerializeField] private Image itemImage = null;
        [SerializeField] private TextMeshProUGUI nameText = null;
        [SerializeField] private TextMeshProUGUI countText = null;

        public void Initialize()
        {
            countText.text = "0";
        }

        public void UpdateSlotUI(InventoryUI ui, Item item, int count) 
        {
            if (count == 0)
                itemButton.onClick.RemoveAllListeners();
            
            //If the count was zero before but now isn't 
            int lastCount = int.Parse(countText.text);
            if (countText.text =="0" || (lastCount == 0 && count > 0))
                itemButton.onClick.AddListener( delegate {
                    ui.ItemClicked(item);
                    item.DoItemStuff();
                    }
                    );

            itemImage.sprite = item.Icon;

            nameText.text = item.ItemName;

            countText.text = count.ToString();
        }
    }
}