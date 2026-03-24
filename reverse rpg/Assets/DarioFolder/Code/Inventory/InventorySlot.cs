using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selected, notselected;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statText;

    public ItemType slotType = ItemType.General;
    [SerializeField] private HealthSystem healthSystem;

    [Header("Special Slot Settings")]
    [SerializeField] private bool isSpecialSlot = false;
    private ItemScript currentItemInSlot;

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
        if (descriptionText == null) return;

        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();
        if (inventoryItem != null && inventoryItem.item != null && !string.IsNullOrEmpty(inventoryItem.item.description))
        {
            descriptionText.text = inventoryItem.item.description;
        }
        else
        {
            descriptionText.text = "";
        }
                if (inventoryItem != null && inventoryItem.item != null && !string.IsNullOrEmpty(inventoryItem.item.stat))
        {
            statText.text = inventoryItem.item.stat;
        }
        else
        {
            statText.text = "";
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
        InventoryItem inventoryItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (inventoryItem == null || inventoryItem.item == null) return;

        if (slotType != ItemType.General && inventoryItem.item.itemType != slotType) return;

        InventoryItem existingItem = GetComponentInChildren<InventoryItem>();

        // Remove old slot effect
        if (isSpecialSlot && currentItemInSlot != null && healthSystem != null)
        {
            healthSystem.RemoveSlotEffect(currentItemInSlot);
            currentItemInSlot = null;
        }

        // Replace existing item if needed
        if (existingItem != null && existingItem != inventoryItem)
        {
            existingItem.transform.SetParent(inventoryItem.parentAfterDrag);
        }

        // Place new item
        inventoryItem.transform.SetParent(transform);
        inventoryItem.transform.SetAsLastSibling();
        inventoryItem.parentAfterDrag = transform;

        UpdateDescriptionText();

        // Apply special slot effect
        if (isSpecialSlot && healthSystem != null)
        {
            currentItemInSlot = inventoryItem.item;
            healthSystem.ApplySlotEffect(currentItemInSlot);
        }
    }
}