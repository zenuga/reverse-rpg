using UnityEngine;
using UnityEngine.InputSystem; // Required for input system compatibility

/// <summary>
/// Handles the main inventory UI toggle functionality.
/// This script manages opening/closing the inventory panel and controls cursor/movement states.
/// When inventory opens: cursor is unlocked and visible, player movement is disabled.
/// When inventory closes: cursor is locked and hidden, player movement is re-enabled.
/// </summary>
public class Inventory : MonoBehaviour
{
    /// <summary>Reference to the inventory UI panel GameObject. Set in Inspector.</summary>
    [SerializeField] private GameObject inventoryUI;
    
    /// <summary>Reference to the PlayerMovement script to disable/enable movement when inventory toggles.</summary>
    [SerializeField] private PlayerMovement movementScript; 

    /// <summary>Tracks whether inventory UI is currently open or closed.</summary>
    private bool isOpen;

    /// <summary>
    /// Called every frame. Checks if the F key was pressed this frame to toggle inventory.
    /// Uses the new InputSystem for keyboard input (more robust than legacy Input class).
    /// </summary>
    void Update()
    {
        // Check if keyboard exists (might be null on some platforms) and if F key was pressed THIS frame only (not held)
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            // Call toggle method to open/close inventory
            ToggleInventory();
        }
    }

    /// <summary>
    /// Toggles the inventory UI between open and closed states.
    /// Also manages cursor lock state and player movement based on inventory state.
    /// </summary>
    private void ToggleInventory()
    {
        // Safety check: if no UI assigned, exit early
        if (inventoryUI == null) return;

        // Toggle the open/closed state (flip the boolean)
        isOpen = !isOpen;
        
        // Show or hide the inventory UI GameObject based on new state
        inventoryUI.SetActive(isOpen);

        // If inventory just opened
        if (isOpen)
        {
            // Unlock cursor so player can click UI elements
            Cursor.lockState = CursorLockMode.None;
            // Make cursor visible on screen
            Cursor.visible = true;

            // Disable player movement while browsing inventory
            if (movementScript != null)
                movementScript.enabled = false;
        }
        // If inventory just closed
        else
        {
            // Lock cursor to center of screen (for first-person-like controls)
            Cursor.lockState = CursorLockMode.Locked;
            // Hide cursor from screen
            Cursor.visible = false;

            // Re-enable player movement when closing inventory
            if (movementScript != null)
                movementScript.enabled = true;
        }
    }
}