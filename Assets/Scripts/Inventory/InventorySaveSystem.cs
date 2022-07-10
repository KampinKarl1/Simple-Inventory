using System.IO; //For reading and writing
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory 
{
    public class InventorySaveSystem : MonoBehaviour
    {
        [SerializeField] private Inventory inventoryToSave = null;

        private static Dictionary<int, Item> allItemCodes = new Dictionary<int, Item>();

        private static int HashItem(Item item) => Animator.StringToHash(item.ItemName);

        const char SPLIT_CHAR = '_';

        private static string FILE_PATH ="NULL!";

        private void Awake()
        {
            FILE_PATH = Application.persistentDataPath + "/Inventory.txt";

            CreateItemDictionary();
        }

        private void OnDisable()
        {
            SaveInventory();
        }

        private void CreateItemDictionary() 
        {
            Item[] allItems = Resources.FindObjectsOfTypeAll<Item>();

            foreach (Item i in allItems) 
            {
                int key = HashItem(i);

                if (!allItemCodes.ContainsKey(key))
                    allItemCodes.Add(key, i);
            }
        }

        public void SaveInventory() 
        {
            using (StreamWriter sw = new StreamWriter(FILE_PATH))
            {
                foreach (KeyValuePair<Item, int> kvp in inventoryToSave.GetInventory)
                {
                    Item item = kvp.Key;
                    int count = kvp.Value;

                    string itemID = HashItem(item).ToString();

                    sw.WriteLine(itemID + SPLIT_CHAR + count);                    
                }
            }
        }
        
        private bool InventorySaveExists() 
        {
            if (!File.Exists(FILE_PATH))
            {
                Debug.LogWarning("The file you're trying to access doesn't exist. (Try saving an inventory first).");
                return false;
            }
            return true;
        }
        
        //Delete all items in the inventory. Will be irreversable. Could just create a new file (ie. Change the name of the old save file and create a new one)
        public void ClearInventorySaveFile() 
        {
            if (!InventorySaveExists())
                return;
                
            File.WriteAllText(FILE_PATH, String.Empty);
        }

        internal Dictionary<Item, int> LoadInventory() 
        {
            if (!InventorySaveExists())
                return;

            Dictionary<Item, int> inventory = new Dictionary<Item, int>();

            string line = "";

            using (StreamReader sr = new StreamReader(FILE_PATH)) 
            {
                while ((line = sr.ReadLine()) != null) 
                {
                    int key = int.Parse( line.Split(SPLIT_CHAR)[0] );
                    Item item = allItemCodes[key];
                    int count = int.Parse(line.Split(SPLIT_CHAR)[1]);

                    inventory.Add(item, count);
                }
            }

            return inventory;
        }
    } 
}
