using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace SimpleInventory.UI
{
    public class ItemSlotUI : MonoBehaviour
    {
        [SerializeField] private Button itemButton = null;
        [SerializeField] private TextMeshProUGUI nameText = null;
        [SerializeField] private TextMeshProUGUI countText = null;
        [SerializeField] private Image itemIcon = null;

        public void UpdateSlotUI(Item i, int count, UnityAction buttonAction)
        {
            nameText.text = i.ItemName;
            countText.text = count.ToString();

            itemIcon.sprite = i.Icon;

            if (buttonAction != null) 
            {
                itemButton.onClick.RemoveAllListeners();
                itemButton.onClick.AddListener(buttonAction);
            }
        }
    }
}