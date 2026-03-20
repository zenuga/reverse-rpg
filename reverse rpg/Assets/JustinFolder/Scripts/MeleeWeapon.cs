using UnityEngine;

public class MeleeWeapon : BaseWeaponScript
{
    public float damage = 20f;
    public float attackRange = 2f;
    public Transform firePoint;
    public LayerMask hitLayer;
    int ignoreLayerMask = ~(1 << 14);
    float attackCooldown = 1f;

    void Update()
    {
        attackCooldown -= Time.deltaTime;
    }
    
    public override void Attack()
    {
        RaycastHit hit;

        if (attackCooldown <= 0f)
        {
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackRange, hitLayer))
            {
            
                HealthSystem healthSystem = hit.collider.GetComponent<HealthSystem>();

                if (healthSystem != null)
                {
                    healthSystem.TakeDamage(damage);
                    Debug.Log("Hit" + hit.collider.gameObject.name);
                }

                else
                {
                 Debug.Log("No HealthSystem found on" + hit.collider.gameObject.name);
                }

            }
            
            attackCooldown = 1f;

        }

        else
        {
            Debug.Log("Attack on cooldown: " + attackCooldown.ToString("F2") + " seconds remaining");
        }
    }
}
