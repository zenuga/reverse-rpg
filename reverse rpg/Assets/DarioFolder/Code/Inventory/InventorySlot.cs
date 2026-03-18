using UnityEngine;
using UnityEngine.EventSystems; // Needed for drag and drop event handling
using UnityEngine.UI; // For the Image component

/// <summary>
/// Represents a single visual slot in the inventory UI.
/// Handles visual feedback (highlight) for selected slots and accepts dropped items.
/// Each slot is a UI element that can hold one InventoryItem.
/// Implements IDropHandler to receive items from drag-and-drop operations.
/// </summary>
public class InventorySlot : MonoBehaviour, IDropHandler
{
    /// <summary>The Image component that displays the slot's background/border.</summary>
    public Image image;
    
    /// <summary>Color used when slot is selected (currently active).</summary>
    public Color selected, notselected;

    /// <summary>
    /// Called when this slot's GameObject is instantiated.
    /// Initializes the slot in deselected (not highlighted) state.
    /// </summary>
    private void Awake()
    {
        // Initialize slot as not selected/highlighted
        deselect();
    }
    /// <summary>
    /// Highlights this slot to show it's currently selected.
    /// Makes the slot background brighter/more opaque.
    /// </summary>
    public void select()
    {
        // Only update if image component exists
        if (image != null)
        {
            // Create white color with high opacity (250/255 ≈ 98% visible)
            Color color = Color.white;
            color.a = 250f / 255f; // 250 opacity - BRIGHT (fully visible, almost)
            // Apply the highlighted color to the slot's background
            image.color = color;
        }
    }
    
    /// <summary>
    /// Removes highlight from this slot to show it's not selected.
    /// Makes the slot background dimmer/less opaque.
    /// </summary>
    public void deselect()
    {
        // Only update if image component exists
        if (image != null)
        {
            // Create white color with lower opacity (210/255 ≈ 82% visible)
            Color color = Color.white;
            color.a = 210f / 255f; // 210 opacity - DIM (less visible, appears grayed out)
            // Apply the dimmed color to the slot's background
            image.color = color;
        }
    }
    /// <summary>
    /// Called when an item is dropped onto this slot.
    /// Accepts the drop if the slot is empty or if the same item is being dropped.
    /// Implements IDropHandler from EventSystems.
    /// </summary>
    /// <param name="eventData">Contains information about the drag operation.</param>
    public void OnDrop(PointerEventData eventData)
    {
        // Get the InventoryItem component from the object being dragged
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        // If dragged object doesn't have an InventoryItem, exit (invalid drop)
        if (inventoryItem == null) return;

        // Check if this slot already has an item in it
        InventoryItem existingItem = GetComponentInChildren<InventoryItem>();
        
        // Accept drop if: slot is empty OR the item is the same object already in this slot
        if (existingItem == null || existingItem == inventoryItem)
        {
            // Move the item to this slot
            inventoryItem.transform.SetParent(transform);
            // Make sure item renders on top of other UI elements in this slot
            inventoryItem.transform.SetAsLastSibling();
            // Update the stored parent reference for future drag operations
            inventoryItem.parentAfterDrag = transform;
        }
    }
}