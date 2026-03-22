using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Text quantityText;

    [HideInInspector] public ItemScript item;
    [HideInInspector] public Transform parentAfterDrag;
    private int quantity = 1;

    private Canvas canvas;
    [SerializeField] private HealthSystem healthSystem;
    
    private bool IsArmorItem(ItemType itemType)
    {
        return itemType == ItemType.Helmet || itemType == ItemType.ChestPiece || 
               itemType == ItemType.Leggings || itemType == ItemType.Boots;
    }

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        
        if (quantityText == null)
        {
            quantityText = GetComponentInChildren<Text>();
        }
        
        if (quantityText == null)
        {
            GameObject textGo = new GameObject("QuantityText");
            textGo.transform.SetParent(transform);
            textGo.transform.localPosition = Vector3.zero;
            
            quantityText = textGo.AddComponent<Text>();
            quantityText.font = Resources.Load<Font>("Arial");
            quantityText.fontSize = 18;
            quantityText.fontStyle = FontStyle.Bold;
            quantityText.alignment = TextAnchor.LowerRight;
            quantityText.color = Color.white;
            
            RectTransform textRect = textGo.GetComponent<RectTransform>();
            textRect.anchoredPosition = new Vector2(-5, 5);
            textRect.sizeDelta = new Vector2(70, 30);
        }
        
        if (healthSystem == null)
        {
            healthSystem = FindObjectOfType<HealthSystem>();
        }
    }

    public void InitialiseItem(ItemScript newItem, int amount = 1)
    {
        if (newItem == null)
        {
            Debug.LogError("❌ InventoryItem: newItem is null!", this);
            return;
        }
        
        if (image == null)
        {
            Debug.LogError("❌ InventoryItem: 'image' field is not assigned! Assign the Image component to this field.", this);
            return;
        }
        
        if (newItem.image == null)
        {
            Debug.LogWarning("⚠️ InventoryItem: ItemScript.image is null! Assign a sprite to the ItemScript asset's 'image' field.", this);
            return;
        }
        
        item = newItem;
        quantity = amount;
        image.sprite = newItem.image;
        image.color = Color.white;
        image.raycastTarget = true;
        
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(45, 45);
        }
        
        UpdateQuantityDisplay();
    }
    
    public void SetQuantity(int amount)
    {
        quantity = Mathf.Max(1, amount);
        UpdateQuantityDisplay();
    }
    
    public void AddQuantity(int amount)
    {
        quantity += amount;
        UpdateQuantityDisplay();
    }
    
    void UpdateQuantityDisplay()
    {
        if (quantityText != null)
        {
            if (quantity > 1)
            {
                quantityText.text = "x" + quantity;
                quantityText.enabled = true;
            }
            else
            {
                quantityText.enabled = false;
            }
        }
    }
    
    public int GetQuantity()
    {
        return quantity;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        
        InventorySlot currentSlot = parentAfterDrag.GetComponent<InventorySlot>();
        if (currentSlot != null && currentSlot.slotType != ItemType.General && IsArmorItem(item.itemType) && healthSystem != null)
        {
            healthSystem.armor -= item.armorBonus;
            healthSystem.DecreaseMaxHealth(item.healthBonus);
        }
        
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject slot = GetSlotUnderPointer(eventData);
        
        if (slot != null)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot == null)
            {
                ReturnToParent();
                return;
            }
            
            if (inventorySlot.slotType != ItemType.General && item.itemType != inventorySlot.slotType)
            {
                ReturnToParent();
                return;
            }
            
            InventoryItem existingItem = slot.GetComponentInChildren<InventoryItem>();
            
            if (existingItem == null || existingItem == this)
            {
                transform.SetParent(slot.transform);
                transform.localPosition = Vector3.zero;
                
                InventorySlot newSlot = slot.GetComponent<InventorySlot>();
                if (newSlot != null && newSlot.slotType != ItemType.General && IsArmorItem(item.itemType) && healthSystem != null)
                {
                    healthSystem.armor += item.armorBonus;
                    healthSystem.IncreaseMaxHealth(item.healthBonus);
                }
            }
            else
            {
                ReturnToParent();
                
                InventorySlot returnSlot = parentAfterDrag.GetComponent<InventorySlot>();
                if (returnSlot != null && returnSlot.slotType != ItemType.General && IsArmorItem(item.itemType) && healthSystem != null)
                {
                    healthSystem.armor += item.armorBonus;
                    healthSystem.IncreaseMaxHealth(item.healthBonus);
                }
            }
        }
        else
        {
            ReturnToParent();
            
            InventorySlot returnSlot = parentAfterDrag.GetComponent<InventorySlot>();
            if (returnSlot != null && returnSlot.slotType != ItemType.General && IsArmorItem(item.itemType) && healthSystem != null)
            {
                healthSystem.armor += item.armorBonus;
                healthSystem.IncreaseMaxHealth(item.healthBonus);
            }
        }
    }

    private void ReturnToParent()
    {
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero;
    }

    private GameObject GetSlotUnderPointer(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var r in results)
        {
            if (r.gameObject.CompareTag("ItemSlot"))
                return r.gameObject;
        }

        return null;
    }
}