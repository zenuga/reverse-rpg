using UnityEngine;
using UnityEngine.EventSystems; // Needed for drag and drop event interfaces
using UnityEngine.UI; // For UI Image and Text components
using System.Collections.Generic; // For List used in raycasting

/// <summary>
/// Represents a single item slot in the inventory UI.
/// Handles visual display of items, quantity indicators, and drag-and-drop functionality.
/// This script implements three drag event interfaces to enable dragging items between slots.
/// </summary>
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    /// <summary>The Image component that displays the item's sprite icon.</summary>
    public Image image;
    /// <summary>Text component that shows item quantity (e.g., "x5" for 5 items).</summary>
    public Text quantityText; // Text for showing item amount

    /// <summary>Reference to the ItemScript ScriptableObject that defines this item's properties.</summary>
    [HideInInspector] public ItemScript item;
    /// <summary>Stores the parent transform before dragging so item can be returned if drop is invalid.</summary>
    [HideInInspector] public Transform parentAfterDrag;
    /// <summary>How many of this item are in this slot (for stackable items).</summary>
    private int quantity = 1;

    /// <summary>Reference to the Canvas (parent UI element) needed for drag positioning.</summary>
    private Canvas canvas;

    /// <summary>
    /// Called when the script instance is being initialized.
    /// Sets up UI components (Image and Text) - finds them on this GameObject or creates them if missing.
    /// This ensures the component works even if not fully set up in the Inspector.
    /// </summary>
    void Awake()
    {
        // Get the Canvas component from parent (needed for UI drag positioning)
        canvas = GetComponentInParent<Canvas>();
        
        // If Image component not assigned in Inspector, try to find it on this GameObject
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        
        // If quantity text not assigned in Inspector, try to find it in child GameObjects
        if (quantityText == null)
        {
            quantityText = GetComponentInChildren<Text>();
        }
        
        // If STILL no quantity text found, create one from scratch as a fallback
        if (quantityText == null)
        {
            // Create a new GameObject to hold the quantity text
            GameObject textGo = new GameObject("QuantityText");
            // Make it a child of this item
            textGo.transform.SetParent(transform);
            // Position it at origin relative to parent
            textGo.transform.localPosition = Vector3.zero;
            
            // Add and configure the Text component
            quantityText = textGo.AddComponent<Text>();
            quantityText.font = Resources.Load<Font>("Arial"); // Load Arial font from Resources folder
            quantityText.fontSize = 18; // Set readable font size
            quantityText.fontStyle = FontStyle.Bold; // Make text bold and readable
            quantityText.alignment = TextAnchor.LowerRight; // Position text in bottom-right of item icon
            quantityText.color = Color.white; // Make text white
            
            // Configure the RectTransform to position and size the text
            RectTransform textRect = textGo.GetComponent<RectTransform>();
            // Position bottom-right with small padding (5 pixels from edges)
            textRect.anchoredPosition = new Vector2(-5, 5); // Bottom-right corner with padding
            // Size the text box to fit the quantity number
            textRect.sizeDelta = new Vector2(70, 30);
        }
    }

    /// <summary>
    /// Initializes this inventory item slot with item data and quantity.
    /// Called whenever an item is added to this slot.
    /// Sets up the visual representation and all necessary properties.
    /// </summary>
    /// <param name="newItem">The ItemScript asset defining this item's properties.</param>
    /// <param name="amount">How many of this item (for stackable items). Defaults to 1.</param>
    public void InitialiseItem(ItemScript newItem, int amount = 1)
    {
        // Validation: Check if the ItemScript reference is valid
        if (newItem == null)
        {
            Debug.LogError("❌ InventoryItem: newItem is null!", this);
            return; // Exit early if no item data
        }
        
        // Validation: Check if the Image component exists (visual display)
        if (image == null)
        {
            Debug.LogError("❌ InventoryItem: 'image' field is not assigned! Assign the Image component to this field.", this);
            return; // Exit early if no image to display
        }
        
        // Validation: Check if the item has a sprite assigned
        if (newItem.image == null)
        {
            Debug.LogWarning("⚠️ InventoryItem: ItemScript.image is null! Assign a sprite to the ItemScript asset's 'image' field.", this);
            return; // Exit early if no sprite to show
        }
        
        // Store reference to the item data
        item = newItem;
        // Set the quantity of this item
        quantity = amount;
        // Display the item's icon sprite in the Image component
        image.sprite = newItem.image;
        // Set color to white so sprite displays normally (not darkened)
        image.color = Color.white; // Ensure it's visible
        // Enable raycast target so drag/drop detection works on this image
        image.raycastTarget = true; // Enable drag/drop interaction
        
        // Configure the RectTransform (UI layout component) to properly position and size the item icon
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null)
        {
            // Set anchor to center (0.5, 0.5) so item scales from its center point
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            // Set pivot to center so rotation/scaling originates from center
            rect.pivot = new Vector2(0.5f, 0.5f);
            // Position at origin relative to parent (the slot)
            rect.anchoredPosition = Vector2.zero;
            // Set item icon size to 45x45 pixels
            rect.sizeDelta = new Vector2(45, 45);
        }
        
        // Update the quantity text display (show "x5" for 5 items, hide for single items)
        UpdateQuantityDisplay();
        // Log confirmation that initialization succeeded
        Debug.Log("Item initialized: " + newItem.itemName);
    }
    
    /// <summary>
    /// Sets the quantity of items in this slot to an exact amount.
    /// Enforces minimum of 1 item (never 0).
    /// </summary>
    /// <param name="amount">The exact quantity to set (will be clamped to minimum 1).</param>
    public void SetQuantity(int amount)
    {
        // Use Mathf.Max to ensure quantity never goes below 1
        quantity = Mathf.Max(1, amount);
        // Update the displayed quantity text
        UpdateQuantityDisplay();
    }
    
    /// <summary>
    /// Increases the quantity of items in this slot by the given amount.
    /// </summary>
    /// <param name="amount">How many items to add (can be negative to remove).</param>
    public void AddQuantity(int amount)
    {
        // Add amount to existing quantity
        quantity += amount;
        // Update the displayed quantity text
        UpdateQuantityDisplay();
    }
    
    /// <summary>
    /// Updates the visual display of quantity text.
    /// Shows "x5" for 5 items, but hides the text for single items (quantity = 1).
    /// </summary>
    void UpdateQuantityDisplay()
    {
        // Only update if the text component exists
        if (quantityText != null)
        {
            // If more than 1 item, show the quantity
            if (quantity > 1)
            {
                // Format as "x" followed by the number
                quantityText.text = "x" + quantity;
                // Make the text visible
                quantityText.enabled = true;
            }
            // If only 1 item, hide the quantity text (single items don't need a label)
            else
            {
                quantityText.enabled = false;
            }
        }
    }
    
    /// <summary>
    /// Gets the current quantity of items in this slot.
    /// </summary>
    /// <returns>The number of items (always at least 1 if slot is occupied).</returns>
    public int GetQuantity()
    {
        return quantity;
    }

    /// <summary>
    /// Called when the user starts dragging this item.
    /// Saves the original parent and moves the item to the canvas so it can appear above slots.
    /// Implements IBeginDragHandler from EventSystems.
    /// </summary>
    /// <param name="eventData">Contains pointer position and UI event data.</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Store the current parent (the slot we're dragging from) so we can return if drop fails
        parentAfterDrag = transform.parent;
        // Move to canvas so item appears above all other UI elements
        transform.SetParent(canvas.transform); // Move to top-level canvas
        // Make this item render on top of other items
        transform.SetAsLastSibling();
    }

    /// <summary>
    /// Called every frame while the user is dragging this item.
    /// Makes the item follow the mouse cursor position.
    /// Implements IDragHandler from EventSystems.
    /// </summary>
    /// <param name="eventData">Contains current pointer position.</param>
    public void OnDrag(PointerEventData eventData)
    {
        // Move the item to follow the pointer position in screen space
        transform.position = eventData.position;
    }

    /// <summary>
    /// Called when the user stops dragging this item.
    /// Checks if item was dropped on a valid inventory slot.
    /// If yes, moves item to that slot. If no, returns item to its original slot.
    /// Implements IEndDragHandler from EventSystems.
    /// </summary>
    /// <param name="eventData">Contains final pointer position and UI event data.</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // Find the inventory slot (if any) that the mouse is currently over
        GameObject slot = GetSlotUnderPointer(eventData);
        
        // If dropped on a valid slot
        if (slot != null)
        {
            // Check if slot already has an item in it
            InventoryItem existingItem = slot.GetComponentInChildren<InventoryItem>();
            
            // Drop is valid if: slot is empty OR the item being dropped is the same as existing (same item)
            if (existingItem == null || existingItem == this)
            {
                // Move this item to the slot
                transform.SetParent(slot.transform);
                // Snap to center of slot
                transform.localPosition = Vector3.zero; // Snap to slot
                // Log for debugging
                Debug.Log("Dropped on slot: " + slot.name);
            }
            else
            {
                // Slot already has a different item, return to original parent
                ReturnToParent();
            }
        }
        else
        {
            // Dropped in empty space (not on a slot), return to original parent
            ReturnToParent();
        }
    }

    /// <summary>
    /// Returns the item to its original slot after a failed drop attempt.
    /// </summary>
    private void ReturnToParent()
    {
        // Set parent back to the slot we started from
        transform.SetParent(parentAfterDrag);
        // Position at center of slot
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Searches for an inventory slot under the current pointer position.
    /// Uses raycasting to find all UI elements under the mouse and checks for ItemSlot tag.
    /// </summary>
    /// <param name="eventData">Contains pointer position and event data.</param>
    /// <returns>The slot GameObject if found, null otherwise.</returns>
    private GameObject GetSlotUnderPointer(PointerEventData eventData)
    {
        // Create a list to store all UI elements under the pointer
        List<RaycastResult> results = new List<RaycastResult>();
        // Raycast from pointer position to find all UI elements in the area
        EventSystem.current.RaycastAll(eventData, results);

        // Loop through all UI elements found
        foreach (var r in results)
        {
            // Check if this element has the "ItemSlot" tag
            if (r.gameObject.CompareTag("ItemSlot")) // Make sure your slots have this tag
                return r.gameObject; // Found a slot, return it
        }

        // No slot found under pointer
        return null;
    }
}