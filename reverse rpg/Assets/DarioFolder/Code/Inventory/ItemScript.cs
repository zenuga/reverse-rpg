using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemScript : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    [TextArea(2, 4)]
    public string description;
    public int maxStackSize = 1;
    
    [Header("Gameplay")]
    public Vector2Int range = new Vector2Int(5, 4);
    public ActionType actionType;
    public ItemType type;
    public int damageValue;
    public TileBase tile;
    public GameObject worldModel; // 3D model to equip in player's hand
    public float weight = 1f;

    [Header("UI")]
    public Sprite image;
    public Color rarity = Color.white;
}

public enum ItemType
{
    BuildingBlock,
    Tool,
    Weapon,
    Armor,
    Consumable
}

public enum ActionType
{
    SpellCast,
    Swing,
    Shoot,
    Block,
    Use
}
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}