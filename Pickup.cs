﻿using UnityEngine;
using UnityEngine.UI;

namespace SimpleInventory 
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private Image overheadImage = null;
        [SerializeField] private Item itemPickup = null;
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
                AddToInventoryAndDestroyThis(collision.gameObject);
        }

        private void AddToInventoryAndDestroyThis() 
        {
            playerObject.GetComponent<Inventory>().AddToInventory(itemPickup, numberPickedUp);
            Destroy(gameObject);
        }
    } 
}
