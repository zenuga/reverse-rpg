using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 150;
    private int currentHealth;
    
    public AudioSource hitSound;    // geluid bij raken

    void Start()
    {
        currentHealth = maxHealth;   // bij start volle health
        
    }

    public void TakeDamage(int damage)   //roept DamageScript aan
    {
        currentHealth -= damage;    // trek health af


        if (hitSound != null)   // speel geluid bij raken
        {
            hitSound.Play();
        }

        if (currentHealth <= 0)     // tject of health op 0 is
        {
            Destroy(gameObject);     // vernietig object
        }
    }
}