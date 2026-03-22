using Unity.VisualScripting;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    [SerializeField]
    private float currentHealth;
    public int armor = 0;
    private int armorBonus = 0;
    
 
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void IncreaseMaxHealth(int amount)
    {
        float healthPercent = currentHealth / maxHealth;
        maxHealth += amount;
        currentHealth = maxHealth * healthPercent;
        armorBonus += amount;
    }
    
    public void DecreaseMaxHealth(int amount)
    {
        maxHealth -= amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        armorBonus -= amount;
    }
    
    public void healthincrease()
    {
        maxHealth = maxHealth + armor;
        currentHealth = maxHealth;
    }

    void Die()
    {
        Debug.Log(gameObject.name + "died.");
        Destroy(gameObject);
    }
}
