using UnityEngine;
using UnityEngine.Rendering;

public class projectile : MonoBehaviour
{
    public float lifeTime;
    public LayerMask enemyLayer;
    public float damage;

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
                Debug.Log("Hit " + collision.gameObject.name + " for " + damage + " damage");
            }
        }
    }
}
