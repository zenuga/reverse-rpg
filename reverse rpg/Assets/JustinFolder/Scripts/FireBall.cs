using UnityEngine;

public class FireBall : projectile
{
    public ParticleSystem explosionEffect;

    void Update()
    {
        damage = 10f;
        lifeTime = 90f;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        
        explode();
    }

    private void explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
