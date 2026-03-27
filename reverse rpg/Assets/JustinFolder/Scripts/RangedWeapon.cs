using System;
using System.Net.Security;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RangedWeapon : BaseWeaponScript
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed;
    public float attackCooldown;
    public float baseAttackCooldown;
    public float attackChargeTime;
    public AudioClip fireSound;
    private float currentAttackCharge;
    private bool isAttacking;
    private AudioSource audioSource;


    

    private void Awake()
    {
        if (firePoint == null)
        {
            Camera cam = Camera.main;

            if (cam != null)
            {
                firePoint = cam.transform;
            }
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UpdateStats()
    {

        base.UpdateStats();

        attackCooldown -= Time.deltaTime;

        if (currentAttackCharge >= attackChargeTime && !Input.GetMouseButton(0) && isAttacking)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.linearVelocity = firePoint.forward * projectileSpeed;

            attackCooldown = baseAttackCooldown;

            if (fireSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(fireSound);
            }

            currentAttackCharge = 0f;

            isAttacking = false;
        }
            
        if (currentAttackCharge <= attackChargeTime && !Input.GetMouseButton(0) && isAttacking)
        {
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

            Debug.Log("Charging attack: " + currentAttackCharge.ToString("F2") + " seconds");

            isAttacking = true;
        }

        else if (attackCooldown >= 0f)
        {
            Debug.Log("Attack is on cooldown: " + attackCooldown);

            isAttacking = false;
        }
    }
}
