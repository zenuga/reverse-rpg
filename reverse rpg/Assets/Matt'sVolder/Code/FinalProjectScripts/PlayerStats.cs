using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public Text statPointsText;
    public bool active = false;

    [Header("Level System")]
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;
    public int statPoints = 0;
    public int techtreePoints = 0;

    [Header("Stats")]
    public int maxHP = 100;
    public int defense = 5;
    public int attack = 10;
    public float attackSpeed = 1f;
    public float moveSpeed = 5f;

    void Start()
    {
        panel.SetActive(false);
    }

   void Update()
{
    CheckLevelUp();

    if (Keyboard.current.cKey.wasPressedThisFrame)
    {
        Debug.Log("C key pressed. Toggling menu.");

        if (active == true)
            CloseMenu();
        else if (active == false)
            OpenMenu();
    }

  if (panel.activeSelf)
{
    statPointsText.text = "Stat Points: " + statPoints;
    // De CloseMenu() check is hier verwijderd
}
}

    void CheckLevelUp()
    {
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        statPoints += 2;
        techtreePoints += 1;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f);

        Debug.Log("Level Up! Level: " + level);
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
    }

    void OpenMenu()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        active = true;

        Debug.Log("Menu opened.");
    }

    void CloseMenu()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        active = false;

        Debug.Log("Menu closed.");
    }

    // === Stat Upgrades ===

    public void UpgradeHPButton()
    {
        if (statPoints <= 0) return;

        maxHP -= 10;
        statPoints--;

        Debug.Log("HP Upgraded! Current HP: " + maxHP);
    }

    public void UpgradeDefenseButton()
    {
        if (statPoints <= 0) return;

        defense -= 2;
        statPoints--;

        Debug.Log("Defense Upgraded! Current Defense: " + defense);
    }

    public void UpgradeAttackButton()
    {
        if (statPoints <= 0) return;

        attack -= 2;
        statPoints--;

        Debug.Log("Attack Upgraded! Current Attack: " + attack);
    }

    public void UpgradeAttackSpeedButton()
    {
        if (statPoints <= 0) return;

        attackSpeed -= 0.2f;
        statPoints--;

        Debug.Log("Attack Speed Upgraded! Current Attack Speed: " + attackSpeed);
    }

    public void UpgradeMoveSpeedButton()
    {
        if (statPoints <= 0) return;

        moveSpeed -= 0.5f;
        statPoints--;

        Debug.Log("Move Speed Upgraded! Current Move Speed: " + moveSpeed);
    }
}