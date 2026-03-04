using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public PlayerStats player;

    public Text statPointsText;

    void Start()
    {
        panel.SetActive(false);
        player.onLevelUp += OpenMenu;
    }

    void Update()
    {
        if (panel.activeSelf)
        {
            statPointsText.text = "Stat Points: " + player.statPoints;

            if (player.statPoints <= 0)
            {
                CloseMenu();
            }
        }
    }

    void OpenMenu()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    void CloseMenu()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    // === Knoppen ===

    public void UpgradeHP()
    {
        player.UpgradeHP();
    }

    public void UpgradeDefense()
    {
        player.UpgradeDefense();
    }

    public void UpgradeAttack()
    {
        player.UpgradeAttack();
    }

    public void UpgradeAttackSpeed()
    {
        player.UpgradeAttackSpeed();
    }

    public void UpgradeMoveSpeed()
    {
        player.UpgradeMoveSpeed();
    }
}