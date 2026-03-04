using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnLevelUp();
    public OnLevelUp onLevelUp;

    [Header("Level System")]
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;
    public int statPoints = 0;

    [Header("Stats")]
    public int maxHP = 100;
    public int defense = 5;
    public int attack = 10;
    public float attackSpeed = 1f;
    public float moveSpeed = 5f;

    void Update()
    {
        CheckLevelUp();
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
    statPoints += 5;
    expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);

    if (onLevelUp != null)
        onLevelUp.Invoke();
}

    public void AddExp(int amount)
    {
        currentExp += amount;
    }

    // === Stat Upgrade Functions ===

    public void UpgradeHP()
    {
        if (statPoints <= 0) return;

        maxHP += 10;
        statPoints -= 1;
    }

    public void UpgradeDefense()
    {
        if (statPoints <= 0) return;

        defense += 2;
        statPoints -= 1;
    }

    public void UpgradeAttack()
    {
        if (statPoints <= 0) return;

        attack += 3;
        statPoints -= 1;
    }

    public void UpgradeAttackSpeed()
    {
        if (statPoints <= 0) return;

        attackSpeed += 0.1f;
        statPoints -= 1;
    }

    public void UpgradeMoveSpeed()
    {
        if (statPoints <= 0) return;

        moveSpeed += 0.5f;
        statPoints -= 1;

        
    }
}