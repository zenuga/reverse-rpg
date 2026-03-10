using System;
using System.Net.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class RangedWeapon : BaseWeaponScript
{
    public abstract GameObject projectilePrefab { get; }
    public abstract Transform firePoint { get; }
    public abstract float projectileSpeed { get; }


    public override void Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * projectileSpeed;

    }

}
