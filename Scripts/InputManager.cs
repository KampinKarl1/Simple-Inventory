using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private Camera mainCam = null;

    [Header("Gives input to these classes")]
    [SerializeField] private SimpleInventory.UI.InventoryUI inventoryUI = null;
    [SerializeField] private SimpleInventory.UI.CraftingUI craftingUI = null;
    [SerializeField] private PlayerMover playerController = null;
    [SerializeField] private SimpleInventory.Miner miner = null;

    [Header("Inputs interpretted")]
    [SerializeField] KeyCode inventoryKey = KeyCode.Tab;
    [SerializeField] KeyCode moveKey = KeyCode.Mouse0;
    [SerializeField] KeyCode miningKey = KeyCode.Mouse1;

    private void Start()
    {
        mainCam = Camera.main;
    
        if (inventoryUI == null || craftingUI == null)
            throw new System.Exception("Place the inventory and crafting UI scripts on the input manager");
    }

    void Update()
    {
        CheckIfMouseOverObject();
    
        if (Input.GetKeyDown(moveKey) && playerController && !EventSystem.current.IsPointerOverGameObject())
            playerController.GiveMoveOrder();

        if (Input.GetKeyDown(inventoryKey))
        {
            inventoryUI.ChangeInventoryState();
            craftingUI.ChangeInventoryState();
        }

         #region Mining
        if (objectMouseIsOver == null)
            return;

        if (Input.GetKey(miningKey))
            miner.MineCurrent(objectMouseIsOver);
        if (Input.GetKeyUp(miningKey))
            miner.ExitMine();
        #endregion
    }
    
        #region MouseOverInformation
    int currentLayer = -1; //The layer the mouse is currently over;
    Ray ray = new Ray(); //memalloc for a ray thatll be fired every frame
    RaycastHit hit = new RaycastHit(); //memalloc
    GameObject objectMouseIsOver = null; //memalloc

    private void CheckIfMouseOverObject() 
    {
        ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            objectMouseIsOver = hit.collider.gameObject;
            currentLayer = objectMouseIsOver.layer;
        }
        else
            objectMouseIsOver = null;
    }
    #endregion
}
