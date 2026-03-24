using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    public int armor = 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(damage - armor, 0);
        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void ApplySlotEffect(ItemScript item)
    {
        if (item == null) return;

        switch (item.slotStatType)
        {
            case SlotStatType.MaxHealth:
                maxHealth += item.slotStatValue;
                currentHealth = Mathf.Min(currentHealth + item.slotStatValue, maxHealth);
                break;

            case SlotStatType.Armor:
                armor += item.slotStatValue;
                break;
        }
    }

    public void RemoveSlotEffect(ItemScript item)
    {
        if (item == null) return;

        switch (item.slotStatType)
        {
            case SlotStatType.MaxHealth:
                maxHealth -= item.slotStatValue;
                currentHealth = Mathf.Min(currentHealth, maxHealth);
                break;

            case SlotStatType.Armor:
                armor -= item.slotStatValue;
                break;
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died.");
        Destroy(gameObject);
    }
}