using UnityEngine;

/// <summary>
/// Marks a GameObject in the world as a pickupable item.
/// This component holds a reference to the ItemScript asset that defines what item this is.
/// Works in conjunction with ItemPickupRaycast which detects and picks up items.
/// 
/// To use:
/// 1. Attach this script to a GameObject in the scene
/// 2. Assign an ItemScript to the itemData field in the Inspector
/// 3. Ensure the GameObject has a Collider component
/// 
/// When the player looks at and presses E on this object, the item is added to inventory.
/// </summary>
public class ItemPickup : MonoBehaviour
{
    /// <summary>Reference to the ItemScript asset that defines this item's properties.</summary>
    public ItemScript itemData;
    
    /// <summary>
    /// Called in the Editor whenever this script is modified (validates setup).
    /// Warns if a Collider is missing, which is required for raycast detection.
    /// </summary>
    private void OnValidate()
    {
        // Check if ItemScript is assigned and if Collider is missing
        if (itemData != null && GetComponent<Collider>() == null)
        {
            // Warn developer that pickup won't work without a Collider
            Debug.LogWarning("ItemPickup needs a Collider component!");
        }
    }
}
