using UnityEngine;
using UnityEngine.InputSystem; // required for the new Input System

public class ChoiceMenuOpen : MonoBehaviour
{
    // this script lives in runtime assembly, so avoid editor namespaces
    public bool IsAllowed = true;
    public GameObject SettingMenu;

    void Update()
    {
        // use Keyboard.current from the InputSystem namespace
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0; // Pause the game
                SettingMenu.SetActive(true); // Show the settings menu
            }
            else
            {
                Time.timeScale = 1; // Resume the game
                SettingMenu.SetActive(false); // Hide the settings menu
            }
        }
    }
}
