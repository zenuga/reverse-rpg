using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    //[SerializeField] private PlayerMovement movementScript; wait till movement script is made to add this back in

    private bool isOpen;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if (inventoryUI == null) return;

        isOpen = !isOpen;
        inventoryUI.SetActive(isOpen);

       // if (isOpen)
       // {
       //     Cursor.lockState = CursorLockMode.None;
       //     Cursor.visible = true;

       //     if (movementScript != null)
       //         movementScript.enabled = false;
       // }
       // else
       // {
      //      Cursor.lockState = CursorLockMode.Locked;
      //      Cursor.visible = false;

       //     if (movementScript != null)
       //         movementScript.enabled = true;
       // }
    }
}