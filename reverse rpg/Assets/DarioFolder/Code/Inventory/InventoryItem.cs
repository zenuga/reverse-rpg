using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemScript item;
    public Transform parentAfterDrag;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Image image;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void InitialiseItem(ItemScript newItem)
    {
        item = newItem;

        if (image != null && item?.image != null)
        {
            image.sprite = item.image;
            image.preserveAspect = true; // ensures correct aspect ratio
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero; // fills slot
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (parentAfterDrag == null) parentAfterDrag = transform.parent;

        // Only allow drag if you want visual dragging (optional)
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Snap back to original parent, disallow switching slots
        transform.SetParent(parentAfterDrag);
        rectTransform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }

    public void ApplySpecialEffect(HealthSystem healthSystem)
    {
        if (item == null || healthSystem == null) return;
        healthSystem.ApplySlotEffect(item);
    }

    public void RemoveSpecialEffect(HealthSystem healthSystem)
    {
        if (item == null || healthSystem == null) return;
        healthSystem.RemoveSlotEffect(item);
    }
}