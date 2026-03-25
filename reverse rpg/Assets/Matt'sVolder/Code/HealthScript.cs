using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 150;
    private int currentHealth;
    private LootDropper lootDropper;

    void Start()
    {
        currentHealth = maxHealth;   // bij start volle health
        lootDropper = GetComponent<LootDropper>(); // krijgt LootDropper component van object, zodat die loot kan droppen als health op 0 is
    }

    public void TakeDamage(int damage)   //roept DamageScript aan
    {
        currentHealth -= damage;    // trek health af

        if (currentHealth <= 0)     // tject of health op 0 is
        {
            lootDropper.DropLoot();   // drop loot
            Destroy(gameObject);     // vernietig object
        }
    }
}