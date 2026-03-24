using UnityEngine;

public abstract class ItemBehaviour : ScriptableObject
{
    public abstract void Use(GameObject user);
}
    
public enum ItemType
{
    General,
    Helmet,
    ChestPiece,
    Leggings,
    Boots,
    Weapon,
    Shield,
    Accessory
}

public enum SlotStatType
{
    None,
    MaxHealth,
    Armor
}

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemScript : ScriptableObject
{
    [Header("Item Info")]
    public ItemType itemType = ItemType.General;
    public string itemName;
    
    [TextArea(2, 4)]
    public string description;
    
    public int maxStackSize = 1;

    [Header("Special Slot Support")]
    public SlotStatType slotStatType = SlotStatType.None;
    public int slotStatValue = 0;

    public GameObject worldModel;
    public MonoBehaviour weaponscript;
    public Sprite image;
}