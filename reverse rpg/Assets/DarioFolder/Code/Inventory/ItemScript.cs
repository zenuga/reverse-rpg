using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

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

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemScript : ScriptableObject
{
    [Header("Item Info")]
    public ItemType itemType = ItemType.General;
    
    public string itemName;
    
    [TextArea(2, 4)]
    public string description;
    
    public int maxStackSize = 1;
    
    [Header("Stats")]
    public int armorBonus = 0;
    public int healthBonus = 0;
    
    public GameObject worldModel;
    
    public MonoBehaviour weaponscript;
    
    public Sprite image;

}
