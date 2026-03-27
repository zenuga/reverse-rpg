using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : RangedWeapon
{
    public override void Init()
    {
        attackChargeTime = 1;
        attackCooldown = 0;
        baseAttackCooldown = 0;
        projectileSpeed = 40;
    }

    public override void Attack()
    {
        base.Attack();
    }
}
