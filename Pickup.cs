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
            if (itemThatGetsPickedUp != null)
                overheadImage.sprite = itemThatGetsPickedUp.Icon;
        }
     
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(playerTag))
                AddToInventoryAndDestroyThis();
        }

        private void AddToInventoryAndDestroyThis() 
        {
        //Get a reference to an inventory and add the item
        
        //You could pass the gameObject collided with and do a GetComponent for the Inventory
            FindObjectOfType<Inventory>().AddToInventory(itemThatGetsPickedUp, numberPickedUp);
            Destroy(gameObject);
        }
    } 
}
