using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [Header("Gives input to these classes")]
    [SerializeField] private SimpleInventory.UI.InventoryUI inventoryUI = null;
    [SerializeField] private SimpleInventory.UI.CraftingUI craftingUI = null;
    [SerializeField] private PlayerMover playerController = null;
    [SerializeField] private Miner miner = null;

    [Header("Inputs interpretted")]
    [SerializeField] KeyCode inventoryKey = KeyCode.Tab;
    [SerializeField] KeyCode moveKey = KeyCode.Mouse0;
    [SerializeField] KeyCode miningKey = KeyCode.Mouse1;

    private void Start()
    {
        if (inventoryUI == null || craftingUI == null)
            throw new System.Exception("Place the inventory and crafting UI scripts on the input manager");
    }

    void Update()
    {
        if (Input.GetKeyDown(moveKey) && playerController && !EventSystem.current.IsPointerOverGameObject())
            playerController.GiveMoveOrder();

        if (Input.GetKeyDown(inventoryKey))
        {
            inventoryUI.ChangeInventoryState();
            craftingUI.ChangeInventoryState();
        }

        if (Input.GetKey(miningKey))
            miner.MineCurrent();
    }
}
