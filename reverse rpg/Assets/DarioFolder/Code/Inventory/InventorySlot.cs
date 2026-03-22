using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel.Design.Serialization;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    CombatControler combatControler;
    
    public Color selected, notselected;
    public TextMeshProUGUI descriptionText;
    public ItemType slotType = ItemType.General;
    [SerializeField] private HealthSystem healthSystem;
    
    private bool IsArmorItem(ItemType itemType)
    {
        return itemType == ItemType.Helmet || itemType == ItemType.ChestPiece || 
               itemType == ItemType.Leggings || itemType == ItemType.Boots;
    }

    private void Awake()
    {
        deselect();
    }
    public void select()
    {
        if (image != null)
        {
            Color color = Color.white;
            color.a = 250f / 255f;
            image.color = color;
        }

        UpdateDescriptionText();
    }
    
    private void UpdateDescriptionText()
    {
        if (descriptionText == null)
            return;

        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();
        if (inventoryItem != null && inventoryItem.item != null && !string.IsNullOrEmpty(inventoryItem.item.description))
        {
            descriptionText.text = inventoryItem.item.description;
        }
        else
        {
            descriptionText.text = "";
        }
    }
    
    public void deselect()
    {
        if (image != null)
        {
            Color color = Color.white;
            color.a = 210f / 255f;
            image.color = color;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (inventoryItem == null) return;

        if (slotType != ItemType.General && inventoryItem.item.itemType != slotType)
        {
            return;
        }

        InventoryItem existingItem = GetComponentInChildren<InventoryItem>();
        
        if (existingItem == null || existingItem == inventoryItem)
        {
            inventoryItem.transform.SetParent(transform);
            inventoryItem.transform.SetAsLastSibling();
            inventoryItem.parentAfterDrag = transform;

            UpdateDescriptionText();
        }
        healthSystem.IncreaseMaxHealth(inventoryItem.item.healthBonus);
    }
}