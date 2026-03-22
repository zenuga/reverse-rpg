using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventorySlot[] inventorySlots;
    
    [SerializeField] private GameObject inventoryItemPrefab;
    
    [SerializeField] private Transform playerHand;
    
    private int selectedSlot = -1;
    
    private GameObject equippedItemInstance;
    
    [SerializeField] private HealthSystem healthSystem;
    
    private ItemScript equippedItem;
    
    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll > 0)
        {
            ChangeSelectedSlot(selectedSlot - 1);
        }
        else if (scroll < 0)
        {
            ChangeSelectedSlot(selectedSlot + 1);
        }
        
        for (int i = 0; i < 8; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ChangeSelectedSlot(i);
            }
        }
    }
    
    void ChangeSelectedSlot(int newValue)
    {
        if (newValue >= 8)
        {
            newValue = 0;
        }
        if (newValue < 0)
        {
            newValue = 7;
        }
        
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].deselect();
        }
        
        inventorySlots[newValue].select();
        selectedSlot = newValue;
        
        ItemScript selectedItem = GetSelectedItem();
        if (selectedItem != null)
        {
            EquipItem(selectedItem);
        }
        else
        {
            UnequipItem();
        }
    }
    
    void EquipItem(ItemScript item)
    {
        UnequipItem();
        
        if (playerHand == null)
        {
            return;
        }
        
        if (item.worldModel == null)
        {
            return;
        }
        
        equippedItemInstance = Instantiate(item.worldModel, playerHand);
        equippedItemInstance.name = item.itemName;
        equippedItemInstance.transform.localPosition = Vector3.zero;
        equippedItemInstance.transform.localRotation = Quaternion.identity;
        
        equippedItem = item;
        
        if (IsArmorItem(item.itemType) && healthSystem != null)
        {
        }
    }
    
    void UnequipItem()
    {
        if (equippedItem != null && IsArmorItem(equippedItem.itemType) && healthSystem != null)
        {
        }
        
        if (equippedItemInstance != null)
        {
            Destroy(equippedItemInstance);
            equippedItemInstance = null;
        }
        
        equippedItem = null;
    }
    
    

    public bool AddItem(ItemScript item)
    {
        if (item == null)
        {
            return false;
        }
        
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            return false;
        }
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            if (slot == null)
            {
                continue;
            }
            
            if (slot.slotType != item.itemType && slot.slotType != ItemType.General)
            {
                continue;
            }
            
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        
        if (item.itemType != ItemType.General)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                if (slot == null)
                {
                    continue;
                }
                
                if (slot.slotType != ItemType.General)
                {
                    continue;
                }
                
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }
        }
        
        return false;
    }
    void SpawnNewItem(ItemScript item, InventorySlot slot)
    {
        if (inventoryItemPrefab == null)
        {
            return;
        }
        
        if (slot == null)
        {
            return;
        }
        
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        newItemGo.name = "InventoryItem_" + item.itemName;
        
        RectTransform itemRect = newItemGo.GetComponent<RectTransform>();
        if (itemRect == null)
        {
            itemRect = newItemGo.AddComponent<RectTransform>();
        }
        
        itemRect.anchorMin = new Vector2(0.5f, 0.5f);
        itemRect.anchorMax = new Vector2(0.5f, 0.5f);
        itemRect.pivot = new Vector2(0.5f, 0.5f);
        itemRect.anchoredPosition = Vector2.zero;
        itemRect.sizeDelta = new Vector2(80, 80);
        
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        if (inventoryItem == null)
        {
            Destroy(newItemGo);
            return;
        }
        
        if (item == null)
        {
            return;
        }
        
        inventoryItem.InitialiseItem(item);
    }
    public ItemScript GetSelectedItem()
    {
        if (selectedSlot < 0 || selectedSlot >= inventorySlots.Length)
        {
            return null;
        }
        
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            return itemInSlot.item;
        }
        return null;
    }
    
    private bool IsArmorItem(ItemType itemType)
    {
        return itemType == ItemType.Helmet || itemType == ItemType.ChestPiece || 
               itemType == ItemType.Leggings || itemType == ItemType.Boots;
    }
}
