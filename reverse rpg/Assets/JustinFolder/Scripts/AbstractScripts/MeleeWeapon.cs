using UnityEngine;

public class MeleeWeapon : BaseWeaponScript
{
    public float attackRange = 2f;
    public LayerMask enemyLayer;

    public override void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, enemyLayer))
        {
            HealthSystem healthSystem = hit.collider.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damage);
            }
        }
    }
}
