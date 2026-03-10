using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selected, notselected;

    private void Awake()
    {
        deselect();
    }
    public void select()
    {
        if (image != null)
        {
            Color color = Color.white;
            color.a = 250f / 255f; // 250 opacity
            image.color = color;
        }
    }
    public void deselect()
    {
        if (image != null)
        {
            Color color = Color.white;
            color.a = 210f / 255f; // 210 opacity
            image.color = color;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (inventoryItem == null) return;

        // Check if slot already has an InventoryItem (not counting other child elements like text)
        InventoryItem existingItem = GetComponentInChildren<InventoryItem>();
        if (existingItem == null || existingItem == inventoryItem)
        {
            inventoryItem.transform.SetParent(transform);
            inventoryItem.transform.SetAsLastSibling();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}