using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public float radius = 1f;   // straal van de explosie
    public int damage = 50;     // schade van de explosie

    void Start()
    {
        // Vind alle objecten in de explosie radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in colliders)     // gaat door alle objecten heen in radius
        {
            Health health = col.GetComponent<Health>();     // vindt health script van object
            if (health != null)     // als die er is roept die TakeDamage aan
            {
                health.TakeDamage(damage);     // geeft schade door
            }
        }
    }

} 