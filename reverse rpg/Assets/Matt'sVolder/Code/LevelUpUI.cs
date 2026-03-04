using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public PlayerStats PlayerStats;
    public bool active = false;
    public Text statPointsText;

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            
            if (active == true)
                CloseMenu();
            else
                OpenMenu();
        }
        if (panel.activeSelf)
        {
            statPointsText.text = "Stat Points: " + PlayerStats.statPoints;

            if (PlayerStats.statPoints <= 0)
            {
                CloseMenu();
            }
        }
    }

    void OpenMenu()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        active = true;
    }

    void CloseMenu()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        active = false;
    }

    // === Knoppen ===

    public void UpgradeHPButton()
    {
        PlayerStats.UpgradeHP();
    }

    public void UpgradeDefenseButton()
    {
        PlayerStats.UpgradeDefense();
    }

    public void UpgradeAttackButton()
    {
        PlayerStats.UpgradeAttack();
    }

    public void UpgradeAttackSpeedButton()
    {
        PlayerStats.UpgradeAttackSpeed();
    }

    public void UpgradeMoveSpeedButton()
    {
        PlayerStats.UpgradeMoveSpeed();
    }
}