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
    public HealthSystem healthSystem;

    [Header("Special Slot Settings")]
    public bool isSpecialSlot = false;

    private ItemScript currentItemInSlot;
    private GameObject currentItemInstance;

    private void Awake()
    {
        deselect();
    }

    private void UpdateDescriptionText()
    {
        if (descriptionText == null) return;

        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();

        if (inventoryItem != null && inventoryItem.item != null)
        {
            descriptionText.text = inventoryItem.item.description ?? "";
            statText.text = inventoryItem.item.stat ?? "";
        }
        else
        {
            descriptionText.text = "";
            statText.text = "";
        }
    }

    public void select()
    {
        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();
        if (inventoryItem == null || inventoryItem.item == null) return;

        if (image != null)
        {
            Color color = Color.white;
            color.a = 250f / 255f;
            image.color = color;
        }

        
        if (currentItemInstance != null)
        {
            Destroy(currentItemInstance);
            currentItemInstance = null;
        }

        
        if (inventoryItem.item.worldModel != null)
        {
            GameObject hand = GameObject.Find("Hand");
            if (hand != null)
            {
                currentItemInstance = Instantiate(inventoryItem.item.worldModel, hand.transform);
                currentItemInstance.transform.localPosition = Vector3.zero;
                currentItemInstance.transform.localRotation = Quaternion.identity;
            }
            else
            {
                Debug.LogWarning("Hand object not found in scene!");
            }
        }

        UpdateDescriptionText();
    }

    public void deselect()
    {
        if (image != null)
        {
            Color color = Color.white;
            color.a = 210f / 255f;
            image.color = color;
        }

        if (currentItemInstance != null)
        {
            Destroy(currentItemInstance);
            currentItemInstance = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (draggedItem == null || draggedItem.item == null) return;

        if (slotType != ItemType.General && draggedItem.item.itemType != slotType) return;

        InventoryItem existingItem = null;
        foreach (Transform child in transform)
        {
            InventoryItem item = child.GetComponent<InventoryItem>();
            if (item != null && item != draggedItem)
            {
                existingItem = item;
                break;
            }
        }

        if (isSpecialSlot && healthSystem != null && existingItem != null && existingItem.item != null)
        {
            healthSystem.RemoveSlotEffect(existingItem.item);
        }

        
        if (existingItem != null && existingItem != draggedItem)
        {
            Transform fallbackParent = draggedItem.parentAfterDrag;

            if (fallbackParent != null)
                existingItem.transform.SetParent(fallbackParent);
        }

        draggedItem.transform.SetParent(transform);
        draggedItem.transform.SetAsLastSibling();
        draggedItem.parentAfterDrag = transform;

        UpdateDescriptionText();

        if (isSpecialSlot && healthSystem != null)
        {
            currentItemInSlot = draggedItem.item;
            healthSystem.ApplySlotEffect(currentItemInSlot);
        }
    }
}