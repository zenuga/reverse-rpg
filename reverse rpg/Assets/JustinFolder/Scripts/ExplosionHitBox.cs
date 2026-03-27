using UnityEngine;

public class ExplosionHitBox : MonoBehaviour
{
    public float damage = 10f;
    public LayerMask enemyLayer;
    public float explosionRadius = 5f;
    public float force = 1500.0f;
    public bool explodeOnStart = false;


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            HealthSystem healthSystem = other.gameObject.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damage);
                Debug.Log("Explosion hit " + other.gameObject.name + " for " + damage + " damage");
            }
            else
            {
                Debug.Log("No HealthSystem found on " + other.gameObject.name);
            }
        }

        ExplosionForce();

        Debug.Log("Explosion collided with: " + other.gameObject.name);
    }

    private void ExplosionForce()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, explosionRadius);
            }
        }
    }
}
