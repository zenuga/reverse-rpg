using System;
using System.Net.Security;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class RangedWeapon : BaseWeaponScript
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed;
    public float attackCooldown;
    public float baseAttackCooldown;
    public float attackChargeTime;
    private float currentAttackCharge;
    private bool isAttacking;

    public override void UpdateStats()
    {
        attackCooldown -= Time.deltaTime;

        if (currentAttackCharge >= attackChargeTime && !Input.GetMouseButton(0) && isAttacking)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.linearVelocity = firePoint.forward * projectileSpeed;

            Debug.Log("Attacking with ranged weapon");

            attackCooldown = baseAttackCooldown;

            currentAttackCharge = 0f;

            isAttacking = false;
        }
            
        if (currentAttackCharge <= attackChargeTime && !Input.GetMouseButton(0) && isAttacking)
        {
            Debug.Log("Attack released before fully charged");

            currentAttackCharge = 0f;

            isAttacking = false;
        }
    }


    public override void Attack()
    {
        base.Attack();

        if (attackCooldown <= 0f)
        {
            currentAttackCharge += Time.deltaTime * 1f;

            Debug.Log("Charging attack:" + currentAttackCharge);

            isAttacking = true;
        }

        else if (attackCooldown >= 0f)
        {
            Debug.Log("Attack is on cooldown: " + attackCooldown);

            isAttacking = false;
        }
    }
}
