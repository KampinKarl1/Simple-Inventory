# Simple-Inventory
Use this simple inventory and pickup system for prototyping your game.

Place all the scripts in your Project/Assets folder. (You could also create the scripts in your project and copy the code from here.)

Item cannot be attached to anything as it is a scriptableobject. 
You will have to right-click/Create/Item to create new items. (Add itemName and icon)

Put the pickup script on a collidable object in your game that has a rigidbody on it. (Place the item SO on the itemToPickup variable)

Put the Inventory on your player or gamemanager or wherever you want really and make sure to plug in the InventoryUI;

Place the InventoryUI script on your canvas but not on the Inventory panel itself (or it wont work when set to inactive).

Put the ItemSlotUI on your ItemSlotUI prefab and make sure to setup the button and texts.
