using UnityEngine;
using UnityEngine.UI;

namespace SimpleInventory 
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private Image overheadImage = null;
        [SerializeField] private Item itemThatGetsPickedUp = null;
        [SerializeField] private int numberPickedUp = 1;

        [SerializeField] string playerTag = "Player";

        private void Start()
        {
            SetImage(itemThatGetsPickedUp);
        }
     
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(playerTag))
                AddToInventoryAndDestroyThis(collision.gameObject);
        }

        private void AddToInventoryAndDestroyThis(GameObject playerObject) 
        {
            playerObject.GetComponent<Inventory>().AddToInventory(itemThatGetsPickedUp, numberPickedUp);
            Destroy(gameObject);
        }

        public void SetItem(Item item, int amount = 1) 
        {
            SetImage(item);

            numberPickedUp = amount;

            itemThatGetsPickedUp = item;
        }

        private void SetImage(Item item) 
        {
            if (item != null && overheadImage != null)
                overheadImage.sprite = item.Icon;
        }
    } 
}
