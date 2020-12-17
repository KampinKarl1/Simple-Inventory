# Simple-Inventory (with simple Factorio-Like crafting and mining)
Check out the tutorials for this project here: https://www.youtube.com/playlist?list=PL8uuriBnyf5ngvXpWUo8h_Fw-vjhDQq3s

Use this simple inventory, crafting, mining, and pickup system for prototyping your game.

----------Assets and Project Settings------------

I mixed up the folders on this repo and folders that should be in Assets are spread out in this repository with Project Settings mixed
in with everything else. My bad. 

PLEASE place the Project Settings folder (or at least the TagManager) in your Unity Project's Project Settings folder and everything
else in the Assets folder.

-------------Items--------------

Item and Craftable (inherits from item) cannot be attached to anything as they are ScriptableObjects.

Right click to create new items and craftables. 

Craftables have an array of crafting ingredients that tells the CraftyCrafter what items are needed and how many of each item are needed to make said craftable - There is nocheck to verify that a Craftable has at least one CraftingIngredient, so enjoy that error.

------------Pickups-------------

Put the pickup script on a collidable object in your game that has a rigidbody on it. 

Plug the type of Item into the itemPickup field in the inspector. You also need to fill in the numberPickedUp field - There is no verification that there is at least one.

This uses OnCollisionEnter to check if the collision is from the Player (by tag) and gets the Inventory from that gameObject.

------------Inventory------------

Put the Inventory on your player. 

Place the CrafyCrafter on your player and plug in the Inventory (via inspector).

-UI-

Place the InventoryUI script on a gameobject containing your inventoryUI but not on the Inventory panel itself (or it will be DISABLED when set to inactive).
Same with the Crafting UI (it's based on the same script as InventoryUI, so you have to plug in the same elements and treat it the same way [don't place it directly 
on crafting UI panel]).

Put the ItemSlotUI script on your ItemSlotUI prefab and make sure to setup the button and texts then plug it in to the CraftingUI and InventoryUI objects.

---------------Mining--------------------

Place miner on your player or whatever units mine in your game 

Mineable is the mine. It holds a finite amount of a resource/item. You could probably use ScriptableObjects for similar mine types (Tree holds TreeConfig with MaxResources, MiningTime, etc)

-UI-

Create a gameobject on your screen overlay canvas with an image as a background for the fill bar, a child image to that set to fill type, and a resource image.

You'll need a WorldSpace Canvas with an Image to mark the current mine.

Ex. for inspecting a unit's mining progress.
      
      class Unit 
        void OnMouseDown()
         UnitManager.InspectUnit (Unit u)  
         
      class MiningUI 
        InitializeUI
            UnitManager.onInspectUnit += UpdateUI;
