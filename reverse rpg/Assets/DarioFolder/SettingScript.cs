using UnityEngine;
using UnityEngine.InputSystem; // required for the new Input System

public class SettingScript : MonoBehaviour
{
    [Tooltip("Reference to the ChoiceMenuOpen component that controls whether choices are allowed")]
    public ChoiceMenuOpen choiceMenuOpenScript;

    public GameObject SettingMenu;
    public GameObject choiceMenu;
    public bool IsInSettingMenu = false;
    public GameObject ChoiceMenu;

    private void Awake()
    {
        if (choiceMenuOpenScript == null)
            choiceMenuOpenScript = GetComponent<ChoiceMenuOpen>();
    }

    public void OnSettingPress()
    {
        SettingMenu.SetActive(true);
        choiceMenu.SetActive(false);
        if (choiceMenuOpenScript != null)
            choiceMenuOpenScript.IsAllowed = false;
        IsInSettingMenu = true;
    }

    public void OnExitPress()
    {
        SettingMenu.SetActive(false);
        choiceMenu.SetActive(true);
        if (choiceMenuOpenScript != null)
            choiceMenuOpenScript.IsAllowed = true;
        IsInSettingMenu = false;
        ChoiceMenu.SetActive(true);
    }

    void Update()
    {
        if (IsInSettingMenu && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OnExitPress();
        }
    }
}
