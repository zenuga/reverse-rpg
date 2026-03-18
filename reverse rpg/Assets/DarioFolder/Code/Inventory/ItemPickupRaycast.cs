using UnityEngine;

/// <summary>
/// Handles picking up items from the game world using raycasting.
/// When the player presses E and looks at an item, this script:
/// 1. Casts a ray from the camera center
/// 2. Checks if it hits an ItemPickup component
/// 3. Adds the item to inventory if there's space
/// 4. Destroys the world object if pickup succeeds
/// 
/// This script should be on the same GameObject as InventoryManager or reference it.
/// </summary>
public class ItemPickupRaycast : MonoBehaviour
{
    /// <summary>Reference to the InventoryManager to add items when picked up.</summary>
    [SerializeField] private InventoryManager inventoryManager;
    
    /// <summary>How far from camera center to raycast for items (in units).</summary>
    [SerializeField] private float raycastDistance = 5f;
    
    /// <summary>LayerMask to filter which layers raycast checks (should include item objects).</summary>
    [SerializeField] private LayerMask itemLayerMask;

    /// <summary>Cached reference to main camera for raycasting. Cached in Start for performance.</summary>
    private Camera mainCamera;

    /// <summary>
    /// Called when the game starts.
    /// Caches camera reference and finds InventoryManager if not assigned.
    /// </summary>
    private void Start()
    {
        // Cache the main camera for raycasting (gets it once instead of every frame)
        mainCamera = Camera.main;
        
        // If InventoryManager not assigned in Inspector, try to find it on this GameObject
        if (inventoryManager == null)
        {
            inventoryManager = GetComponent<InventoryManager>();
        }
    }

    /// <summary>
    /// Called every frame. Checks if E key is pressed to trigger item pickup.
    /// </summary>
    private void Update()
    {
        // Check if E key was pressed THIS frame (not held)
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Attempt to pick up an item in front of the camera
            AttemptItemPickup();
        }
    }

    /// <summary>
    /// Attempts to pick up an item directly in front of the camera.
    /// Uses raycasting from screen center with specified distance and layer mask.
    /// Adds item to inventory if found and inventory has space.
    /// Removes the world object if pickup succeeds.
    /// </summary>
    private void AttemptItemPickup()
    {
        // Create a ray from camera center going forward
        // Screen center is (Screen.width/2, Screen.height/2) - middle of screen
        // Z=0 means on the screen plane
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        
        // Cast the ray out to raycastDistance units, checking only itemLayerMask layers
        // out: contains hit information if something is hit
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, itemLayerMask))
        {
            // We hit something! Try to get the ItemPickup component
            ItemPickup itemPickup = hit.collider.GetComponent<ItemPickup>();
            
            // If the hit object has an ItemPickup component
            if (itemPickup != null)
            {
                // Try to add the item to inventory
                if (inventoryManager.AddItem(itemPickup.itemData))
                {
                    // Success! Item was added to inventory
                    Debug.Log("Picked up: " + itemPickup.itemData.itemName);
                    // Remove the world object now that it's in inventory
                    Destroy(hit.collider.gameObject);
                }
                else
                {
                    // Failed to add item - inventory is full
                    Debug.Log("Inventory is full!");
                }
            }
        }
        // If raycast didn't hit anything, nothing happens (no item in front of camera)
    }
}
