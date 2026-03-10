using UnityEngine;

public abstract class projectile : MonoBehaviour
{
    LayerMask enemyLayer;
    public abstract float damage { get; }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damage);
            }
        }
    }
}
