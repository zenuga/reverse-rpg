using UnityEngine;
using UnityEngine.UI; // For UI components

/// <summary>
/// Manages the entire inventory system including:
/// - Storing references to all inventory slots
/// - Adding items to the inventory
/// - Managing item selection and equipment
/// - Handling hotkey input (number keys 1-8 and mouse scroll wheel)
/// 
/// This is the central hub that coordinates between the UI (InventorySlot) and item handling.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    /// <summary>Array of all inventory slot UI elements. Set in Inspector.</summary>
    [SerializeField] private InventorySlot[] inventorySlots;
    
    /// <summary>Prefab for InventoryItem UI elements. Instantiated when items are added.</summary>
    [SerializeField] private GameObject inventoryItemPrefab;
    
    /// <summary>Transform of the player's hand/equipment point where items are visually equipped.</summary>
    [SerializeField] private Transform playerHand; // Where items are equipped
    
    /// <summary>Index of currently selected slot (-1 means none selected initially).</summary>
    private int selectedSlot = -1;
    
    /// <summary>Instantiated 3D model of currently equipped item being held by player.</summary>
    private GameObject equippedItemInstance; // Currently equipped item visual
    
    /// <summary>
    /// Called every frame. Handles player hotkey input for slot selection:
    /// - Mouse scroll wheel to cycle through slots
    /// - Number keys 1-8 to jump to specific slots
    /// </summary>
    private void Update()
    {
        // Get mouse scroll wheel input (positive = scroll up, negative = scroll down)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        // Scroll up = select previous slot
        if (scroll > 0)
        {
            ChangeSelectedSlot(selectedSlot - 1);
        }
        // Scroll down = select next slot
        else if (scroll < 0)
        {
            ChangeSelectedSlot(selectedSlot + 1);
        }
        
        // Check number keys 1-8 to directly select that slot
        for (int i = 0; i < 8; i++)
        {
            // KeyCode.Alpha1 + i gives us KeyCode.Alpha1, Alpha2, Alpha3... Alpha8
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                // Select the slot corresponding to the number pressed (0-7 for Alpha1-8)
                ChangeSelectedSlot(i);
            }
        }
    }
    
    /// <summary>
    /// Changes the currently selected inventory slot.
    /// Handles wrapping around (slot 8 goes to 0, slot -1 goes to 7).
    /// Updates visual selection highlight and equips/unequips items as needed.
    /// </summary>
    /// <param name="newValue">The slot index to select (0-7).</param>
    void ChangeSelectedSlot(int newValue)
    {
        // Wrap forward: if going past slot 7, go back to slot 0
        if (newValue >= 8)
        {
            newValue = 0;
        }
        // Wrap backward: if going below slot 0, go to slot 7
        if (newValue < 0)
        {
            newValue = 7;
        }
        
        // Deselect the previously selected slot (visual state)
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].deselect(); // Make previously selected slot look unselected
        }
        
        // Select the new slot (visual state)
        inventorySlots[newValue].select(); // Make new slot look selected (highlighted)
        // Update tracking variable
        selectedSlot = newValue;
        
        // Get the item in the newly selected slot
        ItemScript selectedItem = GetSelectedItem();
        // If there's an item in this slot, equip it (show 3D model in player's hand)
        if (selectedItem != null)
        {
            EquipItem(selectedItem);
        }
        // If slot is empty, unequip the current item
        else
        {
            UnequipItem();
        }
    }
    
    /// <summary>
    /// Equips an item by instantiating its 3D world model in the player's hand.
    /// First unequips any previously equipped item.
    /// </summary>
    /// <param name="item">The ItemScript defining the item to equip.</param>
    void EquipItem(ItemScript item)
    {
        // Remove any currently equipped item first
        UnequipItem();
        
        // Safety check: verify player hand transform exists
        if (playerHand == null)
        {
            return; // Can't equip without a hand position
        }
        
        // Safety check: verify item has a 3D world model to display
        if (item.worldModel == null)
        {
            return; // Can't display item without a 3D model
        }
        
        // Create 3D instance of the item's world model in the player's hand
        equippedItemInstance = Instantiate(item.worldModel, playerHand);
        // Set the display name for debugging
        equippedItemInstance.name = item.itemName;
        // Position the model at the hand's origin (relative to hand)
        equippedItemInstance.transform.localPosition = Vector3.zero;
        // Set the model's rotation to match the hand (no offset rotation)
        equippedItemInstance.transform.localRotation = Quaternion.identity;
    }
    
    /// <summary>
    /// Removes the currently equipped item by destroying its 3D world model instance.
    /// </summary>
    void UnequipItem()
    {
        // If an item is currently equipped
        if (equippedItemInstance != null)
        {
            // Destroy the 3D model GameObject
            Destroy(equippedItemInstance);
            // Clear the reference
            equippedItemInstance = null;
        }
    }
    
    

    /// <summary>
    /// Adds an item to the first available empty inventory slot.
    /// Returns true if successful, false if no empty slots available.
    /// </summary>
    /// <param name="item">The ItemScript to add to inventory.</param>
    /// <returns>True if item was added, false if inventory is full.</returns>
    public bool AddItem(ItemScript item)
    {
        // Validation: check if item is valid
        if (item == null)
        {
            return false; // Can't add null item
        }
        
        // Validation: check if slots array exists and has slots
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            return false; // No inventory to add to
        }
        
        // Loop through all inventory slots looking for an empty one
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            // Skip null slots (shouldn't happen but good to check)
            if (slot == null)
            {
                continue; // Go to next slot
            }
            
            // Check if this slot already contains an item
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            // If slot is empty
            if (itemInSlot == null)
            {
                // Add item to this slot
                SpawnNewItem(item, slot);
                return true; // Success
            }
        }
        // Looped through all slots and none were empty
        return false; // Inventory is full
    }
    /// <summary>
    /// Creates a new InventoryItem UI element in the given slot.
    /// Instantiates prefab, configures its layout, and initializes it with item data.
    /// </summary>
    /// <param name="item">The ItemScript to display.</param>
    /// <param name="slot">The InventorySlot to add the item to.</param>
    void SpawnNewItem(ItemScript item, InventorySlot slot)
    {
        // Validation: check if prefab exists
        if (inventoryItemPrefab == null)
        {
            return; // Can't create without prefab
        }
        
        // Validation: check if slot exists
        if (slot == null)
        {
            return; // Can't add to null slot
        }
        
        // Create instance of the InventoryItem prefab as a child of the slot
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        // Name it after the item for debugging
        newItemGo.name = "InventoryItem_" + item.itemName;
        
        // Get or create the RectTransform (required for UI positioning)
        RectTransform itemRect = newItemGo.GetComponent<RectTransform>();
        if (itemRect == null)
        {
            itemRect = newItemGo.AddComponent<RectTransform>(); // Add if doesn't exist
        }
        
        // Configure the RectTransform to center the item in the slot and size it properly
        // Set anchor to center
        itemRect.anchorMin = new Vector2(0.5f, 0.5f);
        itemRect.anchorMax = new Vector2(0.5f, 0.5f);
        // Set pivot to center
        itemRect.pivot = new Vector2(0.5f, 0.5f);
        // Position at center of slot
        itemRect.anchoredPosition = Vector2.zero;
        // Size the item UI element
        itemRect.sizeDelta = new Vector2(80, 80);
        
        // Get the InventoryItem component that handles the logic
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        if (inventoryItem == null)
        {
            // If InventoryItem component is missing, clean up and exit
            Destroy(newItemGo);
            return;
        }
        
        // Final validation
        if (item == null)
        {
            return; // Can't initialize without item data
        }
        
        // Initialize the InventoryItem with the item data
        inventoryItem.InitialiseItem(item);
    }
    /// <summary>
    /// Gets the ItemScript of the currently selected inventory slot.
    /// </summary>
    /// <returns>The ItemScript of selected slot, or null if no item selected or slot is empty.</returns>
    public ItemScript GetSelectedItem()
    {
        // Check if selectedSlot is in valid range
        if (selectedSlot < 0 || selectedSlot >= inventorySlots.Length)
        {
            return null; // No valid selection
        }
        
        // Get the slot at the selected index
        InventorySlot slot = inventorySlots[selectedSlot];
        // Get the InventoryItem UI element in this slot (if any)
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        // If slot contains an item
        if (itemInSlot != null)
        {
            // Return the ItemScript data
            return itemInSlot.item;
        }
        // Slot is empty
        return null;
    }
}
