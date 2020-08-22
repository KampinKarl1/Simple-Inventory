using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        [SerializeField] private string itemName = "Write a name";
        [SerializeField] private Sprite icon = null;

        public Sprite Icon => icon;
        public string ItemName => itemName;

        public void DoItemStuff() 
        {
            string player = GameObject.FindGameObjectWithTag("Player").name;

            Debug.Log($"{player} just got a sweet buff or whatever");
        }
    }
}