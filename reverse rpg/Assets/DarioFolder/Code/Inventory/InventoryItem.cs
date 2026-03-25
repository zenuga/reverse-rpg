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
            image.preserveAspect = true;

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
{
    if (parentAfterDrag == null)
        parentAfterDrag = transform.parent;

    InventorySlot slot = parentAfterDrag.GetComponent<InventorySlot>();

    // REMOVE effect when leaving special slot
    if (slot != null && slot.isSpecialSlot)
    {
        if (slot.healthSystem != null && item != null)
        {
            slot.healthSystem.RemoveSlotEffect(item);
        }
    }

    canvasGroup.blocksRaycasts = false;
}

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        rectTransform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }
}