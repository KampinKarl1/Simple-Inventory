# Simple-Inventory (and simple Factorio-Like crafting)
Use this simple inventory, crafting, and pickup system for prototyping your game.

Place all the scripts in your Project/Assets folder. (You could also create the scripts in your project and copy the code from here.)

Item and Craftable (inherits from item) cannot be attached to anything as they are ScriptableObjects.

Right click to create new items and craftables. 

Craftables have an array of crafting ingredients - I don't think there is any check to verify that a Craftable has at least one CraftingIngredient, so enjoy that error.

Put the pickup script on a collidable object in your game that has a rigidbody on it. (Place the item SO on the itemToPickup variable)

Put the Inventory on your player or gamemanager or wherever you want really and make sure to plug in the InventoryUI;
Place the CrafyCrafter on your player and plug in CraftingUI and the Inventory.

Place the InventoryUI script on your canvas but not on the Inventory panel itself (or it wont work when set to inactive).
Same with the Crafting UI (it's based on the same script as InventoryUI, so you have to plug in the same elements and treat it the same way [don't place it directly 
on crafting UI panel]).

Put the ItemSlotUI script on your ItemSlotUI prefab and make sure to setup the button and texts.
