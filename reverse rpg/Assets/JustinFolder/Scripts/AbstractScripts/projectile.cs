using UnityEngine;

public abstract class projectile : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    public abstract float damage { get; }
    private float lifeTime = 5f;

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

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
        Destroy(gameObject);
    }
}
