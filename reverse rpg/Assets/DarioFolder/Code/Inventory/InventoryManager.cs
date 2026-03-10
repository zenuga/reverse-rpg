using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private Transform playerHand; // Where items are equipped
    private int selectedSlot = -1;
    private GameObject equippedItemInstance; // Currently equipped item visual
    
    private void OnEnable()
    {
        // Validate that required fields are assigned
        if (inventoryItemPrefab == null)
        {
            Debug.LogError("InventoryManager: inventoryItemPrefab is not assigned! Assign it in the inspector.", this);
        }
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            Debug.LogError("InventoryManager: inventorySlots is not assigned! Assign it in the inspector.", this);
        }
    }
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
        
        // Number keys 1-8 to select slots
        for (int i = 0; i < 8; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ChangeSelectedSlot(i);
            }
        }
        
        // Activate selected item with right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            ActivateSelectedItem();
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
            Debug.Log("Selected: " + selectedItem.itemName);
            EquipItem(selectedItem);
        }
        else
        {
            UnequipItem();
        }
    }
    
    void EquipItem(ItemScript item)
    {
        // Remove previously equipped item
        UnequipItem();
        
        if (playerHand == null)
        {
            Debug.LogWarning("Player hand not assigned! Assign it in the InventoryManager inspector.");
            return;
        }
        
        if (item.worldModel == null)
        {
            Debug.LogWarning("Item " + item.itemName + " has no 3D model assigned!");
            return;
        }
        
        // Instantiate the item in the player's hand
        equippedItemInstance = Instantiate(item.worldModel, playerHand);
        equippedItemInstance.name = item.itemName;
        equippedItemInstance.transform.localPosition = Vector3.zero;
        equippedItemInstance.transform.localRotation = Quaternion.identity;
    }
    
    void UnequipItem()
    {
        if (equippedItemInstance != null)
        {
            Destroy(equippedItemInstance);
            equippedItemInstance = null;
        }
    }
    
    void ActivateSelectedItem()
    {
        ItemScript item = GetSelectedItem();
        if (item != null)
        {
            Debug.Log("⚡ Activated: " + item.itemName);
            UseItem(item);
        }
        else
        {
            Debug.Log("❌ No item selected!");
        }
    }
    
    void UseItem(ItemScript item)
    {
        // Add your item usage logic here
        switch (item.actionType)
        {
            case ActionType.Swing:
            case ActionType.Shoot:
            case ActionType.SpellCast:
            case ActionType.Block:
            case ActionType.Use:
                Debug.Log("Item action: " + item.actionType + " (" + item.itemName + ")");
                break;
        }
    }
    public bool AddItem(ItemScript item)
    {
        if (item == null)
        {
            Debug.LogError("❌ AddItem: item is null!");
            return false;
        }
        
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            Debug.LogError("❌ AddItem: inventorySlots is not assigned or empty! Assign it in the inspector.");
            return false;
        }
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            if (slot == null)
            {
                Debug.LogError("❌ Slot " + i + " is null!");
                continue;
            }
            
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }
    void SpawnNewItem(ItemScript item, InventorySlot slot)
    {
        if (inventoryItemPrefab == null)
        {
            Debug.LogError("❌ InventoryManager: inventoryItemPrefab is NOT assigned in the inspector!", this);
            return;
        }
        
        if (slot == null)
        {
            Debug.LogError("❌ InventoryManager: slot is null!", this);
            return;
        }
        
        // Instantiate the InventoryItem prefab directly in the slot
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        newItemGo.name = "InventoryItem_" + item.itemName;
        
        // Ensure RectTransform exists
        RectTransform itemRect = newItemGo.GetComponent<RectTransform>();
        if (itemRect == null)
        {
            itemRect = newItemGo.AddComponent<RectTransform>();
        }
        
        // Center the item in the slot
        itemRect.anchorMin = new Vector2(0.5f, 0.5f);
        itemRect.anchorMax = new Vector2(0.5f, 0.5f);
        itemRect.pivot = new Vector2(0.5f, 0.5f);
        itemRect.anchoredPosition = Vector2.zero;
        itemRect.sizeDelta = new Vector2(80, 80);
        
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        if (inventoryItem == null)
        {
            Debug.LogError("❌ InventoryManager: InventoryItem script not found on prefab! Add InventoryItem script to your prefab.", this);
            Destroy(newItemGo);
            return;
        }
        
        if (item == null)
        {
            Debug.LogError("❌ InventoryManager: item is null!", this);
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
}
