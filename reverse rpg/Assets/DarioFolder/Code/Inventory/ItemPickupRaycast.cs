using UnityEngine;

public class ItemPickupRaycast : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask itemLayerMask;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        
        if (inventoryManager == null)
        {
            inventoryManager = GetComponent<InventoryManager>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptItemPickup();
        }
    }
    private void AttemptItemPickup()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, itemLayerMask))
        {
            ItemPickup itemPickup = hit.collider.GetComponent<ItemPickup>();
            
            if (itemPickup != null)
            {
                if (inventoryManager.AddItem(itemPickup.itemData))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
