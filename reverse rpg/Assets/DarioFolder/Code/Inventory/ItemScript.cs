using UnityEngine;
using UnityEngine.Tilemaps; // Imported but not used - can be removed

/// <summary>
/// ScriptableObject that defines all properties for an inventory item.
/// This acts as a data container for item information used throughout the inventory system.
/// 
/// To create a new item:
/// 1. Right-click in Project folder
/// 2. Select: Create > Scriptable Object > Item
/// 3. Fill in all the fields
/// 4. Save the asset
/// 
/// The item can then be placed in the world (ItemPickup) or referenced in code.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable object/Item")] // Makes right-click > Create menu option
public class ItemScript : ScriptableObject
{
    [Header("Item Info")] // Creates a visual section header in Inspector
    
    /// <summary>Display name of the item (shown in UI).</summary>
    public string itemName;
    
    /// <summary>Long description of the item (shown when item is selected).</summary>
    [TextArea(2, 4)] // Creates a taller text area in Inspector (2-4 lines)
    public string description;
    
    /// <summary>Maximum number of this item that can stack in one inventory slot.</summary>
    public int maxStackSize = 1;
    
    /// <summary>3D model prefab to display when item is equipped in player's hand.</summary>
    public GameObject worldModel; // 3D model to equip in player's hand
    
    /// <summary>Weight of the item (in game units). Used for inventory system calculations.</summary>
    public float weight = 1f;
    
    /// <summary>2D sprite icon shown in inventory UI slots.</summary>
    public Sprite image;

}
