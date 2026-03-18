using UnityEngine;
using UnityEngine.Rendering;

public class projectile : MonoBehaviour
{
    public virtual float lifeTime { get; set; }
    public LayerMask enemyLayer;
    public virtual float damage { get; }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
 

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damage);
                Debug.Log("Hit");
            }
            else
            {
                Debug.Log("No HealthSystem found on " + collision.gameObject.name);
            }
        }
        Debug.Log("Collided with: " + collision.gameObject.name);
    }
}
