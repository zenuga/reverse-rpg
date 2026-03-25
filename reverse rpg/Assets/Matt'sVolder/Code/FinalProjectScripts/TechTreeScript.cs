using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TechTreeScript : MonoBehaviour
{
    [Header("UI")]
    public GameObject techtreePanel;
    public Text techtreeText;
    public Button button;
    public Button higherButton;

    [Header("Player")]
    public PlayerStats player;
    public float misfireChance = 0.01f; 
    public float misfireChanceSecondUpgradeButton = 0.01f;
    int techtreePoints = 0;

    void Start()
    {
        techtreePanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            if (techtreePanel.activeSelf)
                CloseMenu();
            else
                OpenMenu();
        }

        techtreeText.text = "Tech Tree Points: " + player.techtreePoints;
    }

    void OpenMenu()
    {
        techtreePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void CloseMenu()
    {
        techtreePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // ===== Tech Tree Buttons =====

 public void ChanceToMissfire()
{
    if (player.techtreePoints > 0)
    {
        player.techtreePoints--;

        misfireChance -= 0.01f;

        Debug.Log("Chance to Misfire upgraded! Current chance: " + misfireChance);

        button.interactable = false;
    }
    else
    {
        Debug.Log("Not enough Tech Tree Points!");
    }
}

public void ChanceToMissfirehigher()
{
    if (player.techtreePoints > 0)
    {
        player.techtreePoints--;

        misfireChance -= 0.01f;

        Debug.Log("Chance to Misfire upgraded! Current chance: " + misfireChance);

        higherButton.interactable = false;
    }
    else
    {
        Debug.Log("Not enough Tech Tree Points!");
    }
}

} 